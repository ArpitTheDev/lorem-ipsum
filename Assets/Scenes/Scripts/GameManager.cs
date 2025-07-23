using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Queue<Card> CardQueue = new Queue<Card>();
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

        if (first.CardUnFolded == second.CardUnFolded)
        {
           Destroy(first.gameObject);
           Destroy(second.gameObject);
        }
        else
        {
            first.FlipBack();
            second.FlipBack();
        }
    }
}
