using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CardSpawner CardSpawner;
    Queue<Card> CardQueue = new Queue<Card>();

    int MatchCounter;
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
            Destroy(first.gameObject);
            Destroy(second.gameObject);

            MatchCounter++;

            bool IsGameOver = CheckGameOver();

            if (!IsGameOver)
            {
                AudioManager.Instance.PlayMatch();
            }
        }
        else
        {
            AudioManager.Instance.PlayMismatch();

            first.FlipBack();
            second.FlipBack();
        }
    }

    bool CheckGameOver() 
    {
        if (MatchCounter == CardSpawner.TotalCards / 2) 
        {
            //Debug.Log("Game Over");
            AudioManager.Instance.PlayGameOver();
            return true;
        }
            
        return false;
    }
}
