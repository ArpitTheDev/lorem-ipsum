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
        SaveData data = new SaveData
        {
            Score = Score,
            MatchCounter = MatchCounter,
            MatchMultiplierCounter = MatchMultiplyerCounter,
            Rows = CardSpawner.rows,
            Cols = CardSpawner.cols,
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
            data.CardsState.Add(cData);
        }

        SaveManager.SaveGame(data);
    }

    SaveData LoadProgress()
    {
        SaveData data = SaveManager.LoadGame();
        if (data == null)
            return null;

        Score = data.Score;
        GamePlayUI.SetScoreText(Score);
        MatchCounter = data.MatchCounter;
        MatchMultiplyerCounter = data.MatchMultiplierCounter;
        CardSpawner.rows = data.Rows;
        CardSpawner.cols = data.Cols;

        return data;
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
        Card first = CardQueue.Dequeue();
        Card second = CardQueue.Dequeue();

        if (first.CardShown == second.CardShown)
        {
            CardSpawner.SpawnedCards.Remove(first);
            CardSpawner.SpawnedCards.Remove(second);

            Destroy(first.gameObject);
            Destroy(second.gameObject);

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

            first.FlipBack();
            second.FlipBack();
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
