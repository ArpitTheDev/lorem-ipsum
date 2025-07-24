using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public static event Action<Card> OnFlipCompleted;

    public SpriteRenderer SR;
    public Sprite CardHidden;
    public Sprite CardShown;

    public float CardFlipAnimDuration;

    private void Awake()
    {
        SR.sprite = CardHidden;
    }

    public void OnCardClicked()
    {
        Flip();
    }

    public bool IsCardUnFolded;
    public bool IsFlipAnimating;

    public void Flip()
    {
        if (!IsCardUnFolded && !IsFlipAnimating)
            StartCoroutine(FlipAnimation(CardShown));
    }

    public void FlipBack()
    {
        if (IsCardUnFolded && !IsFlipAnimating)
            StartCoroutine(FlipAnimation(CardHidden));
    }

    private IEnumerator FlipAnimation(Sprite newSprite)
    {
        AudioManager.Instance.PlayFlip();
        IsFlipAnimating = true;
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
        IsFlipAnimating = false;

        if (IsCardUnFolded)
            OnFlipCompleted?.Invoke(this);

        
    }
}
