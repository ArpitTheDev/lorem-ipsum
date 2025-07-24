using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CardSpawner CardSpawner;
    Queue<Card> CardQueue = new Queue<Card>();

    public int ScoreForMatch;
    int MatchMultiplyerCounter;
    int Score;

    int MatchCounter;

    public GamePlayUI GamePlayUI;
    public GameOverUI GameOverUI;

    void Start()
    {
       if(MainMenu.CurrentGameStateSelected == GameState.OldGame)
            CardSpawner.GenerateBoard(LoadProgress()); 
       else
            CardSpawner.GenerateBoard(null);
    }

    void OnApplicationQuit()
    {
        if (CardSpawner.SpawnedCards.Count > 0) SaveProgress();
        else SaveManager.DeleteSave();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            if(CardSpawner.SpawnedCards.Count > 0) SaveProgress(); 
            else SaveManager.DeleteSave();
        }
    }

    void SaveProgress()
    {
        SaveData Data = new SaveData
        {
            Score = Score,
            MatchCounter = MatchCounter,
            MatchMultiplierCounter = MatchMultiplyerCounter,
            Rows = CardSpawner.Rows,
            Cols = CardSpawner.Cols,
            CardSpawnerScaleX = CardSpawner.transform.localScale.x,
            CardSpawnerScaleY = CardSpawner.transform.localScale.y,
            CardsState = new List<CardData>()
        };

        foreach (Card card in CardSpawner.SpawnedCards)
        {
            CardData cData = new CardData
            {
                CardShownName = card.CardShown.name,
                CardLocation = card.transform.localPosition
            };
            Data.CardsState.Add(cData);
        }

        SaveManager.SaveGame(Data);
    }

    SaveData LoadProgress()
    {
        SaveData Data = SaveManager.LoadGame();
        if (Data == null)
            return null;

        Score = Data.Score;
        GamePlayUI.SetScoreText(Score);
        MatchCounter = Data.MatchCounter;
        MatchMultiplyerCounter = Data.MatchMultiplierCounter;
        CardSpawner.Rows = Data.Rows;
        CardSpawner.Cols = Data.Cols;

        return Data;
    }

    void OnEnable()
    {
        Card.OnFlipCompleted += HandleCardFlipped;
    }

    void OnDisable()
    {
        Card.OnFlipCompleted -= HandleCardFlipped;
    }

    private void HandleCardFlipped(Card card)
    {
        CardQueue.Enqueue(card);

        if (CardQueue.Count >= 2)
        {
            CheckMatch();
        }
    }

    void CheckMatch()
    {
        Card First = CardQueue.Dequeue();
        Card Second = CardQueue.Dequeue();

        if (First.CardShown == Second.CardShown)
        {
            CardSpawner.SpawnedCards.Remove(First);
            CardSpawner.SpawnedCards.Remove(Second);

            Destroy(First.gameObject);
            Destroy(Second.gameObject);

            MatchCounter++;
            MatchMultiplyerCounter++;

            Score += ScoreForMatch * MatchMultiplyerCounter;

            GamePlayUI.SetScoreText(Score);

            bool IsGameOver = CheckGameOver();

            Debug.Log(Score);

            if (!IsGameOver)
                AudioManager.Instance.PlayMatch();
        }
        else
        {
            MatchMultiplyerCounter = 0;
            AudioManager.Instance.PlayMismatch();

            First.FlipBack();
            Second.FlipBack();
        }
    }

    bool CheckGameOver() 
    {
        if (MatchCounter == CardSpawner.TotalCards / 2) 
        {
            AudioManager.Instance.PlayGameOver();
            SaveManager.DeleteSave();

            GamePlayUI.gameObject.SetActive(false);
            GameOverUI.gameObject.SetActive(true);

            GameOverUI.SetScoreText(Score);
            return true;
        }
            
        return false;
    }
}
