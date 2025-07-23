using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public static event Action<Card> OnFlipCompleted;

    public SpriteRenderer SR;
    public Sprite CardFolded;
    public Sprite CardUnFolded;

    public float CardFlipAnimDuration;

    private void Awake()
    {
        SR.sprite = CardFolded;
    }

    public void OnCardClicked()
    {
        Flip();
    }

    public bool IsCardUnFolded;
    public void Flip()
    {
        if (!IsCardUnFolded)
            StartCoroutine(FlipAnimation(CardUnFolded));
    }

    public void FlipBack()
    {
        if (IsCardUnFolded)
            StartCoroutine(FlipAnimation(CardFolded));
    }

    private IEnumerator FlipAnimation(Sprite newSprite)
    {
        float halfDuration = CardFlipAnimDuration / 2f;
        Vector3 scale = transform.localScale;

        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            float lerp = 1 - (t / halfDuration);
            transform.localScale = new Vector3(lerp * scale.x, scale.y, scale.z);
            yield return null;
        }

        SR.sprite = newSprite;

        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            float lerp = t / halfDuration;
            transform.localScale = new Vector3(lerp * scale.x, scale.y, scale.z);
            yield return null;
        }

        transform.localScale = scale;

        IsCardUnFolded = !IsCardUnFolded;

        if(IsCardUnFolded)
            OnFlipCompleted?.Invoke(this);
    }
}
