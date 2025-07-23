using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SpriteRenderer SR;
    public Sprite CardFolded;
    public Sprite CardUnFolded;

    private void Awake()
    {
        SR.sprite = CardFolded;
    }
    public void OnClicked()
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
        IsCardUnFolded = false;
    }

    private IEnumerator FlipAnimation(Sprite newSprite)
    {
        float duration = 0.1f;

        Vector3 scale = transform.localScale;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float lerp = 1 - (t / duration);
            transform.localScale = new Vector3(lerp * scale.x, scale.y, scale.z);
            yield return null;
        }

        SR.sprite = newSprite;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float lerp = t / duration;
            transform.localScale = new Vector3(lerp * scale.x, scale.y, scale.z);
            yield return null;
        }

        transform.localScale = scale;

        IsCardUnFolded = !IsCardUnFolded;
    }
}
