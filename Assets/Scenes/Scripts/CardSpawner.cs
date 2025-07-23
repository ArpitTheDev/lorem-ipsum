using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public Card cardPrefab;

    public int rows = 2;
    public int cols = 2;

    public float Spacing = 2f;

    public float CardWidth;     
    public float CardHeight;

    void Start()
    {
        CenterSpawner();
        GenerateBoard();
        SpriteRenderer sr = cardPrefab.GetComponent<SpriteRenderer>();
        CardWidth = sr.bounds.size.x;
        CardHeight = sr.bounds.size.y;
    }

    void CenterSpawner()
    {
        Vector3 camPos = Camera.main.transform.position;
        transform.position = new Vector3(camPos.x, camPos.y, 0f);
    }


    void GenerateBoard()
    {
        int totalCards = rows * cols;

        // Generate paired IDs and shuffle
        List<int> ids = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }
        ids = Shuffle(ids);

        // Compute total grid size
        float gridWidth = cols * (CardWidth + Spacing) - Spacing;
        float gridHeight = rows * (CardHeight + Spacing) - Spacing;

        // Get screen dimensions in world space
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        float maxWidth = screenWidth * 0.9f;
        float maxHeight = screenHeight * 0.9f;

        // Calculate scale factor to fit within screen
        float scaleX = maxWidth / gridWidth;
        float scaleY = maxHeight / gridHeight;
        float scale = Mathf.Min(scaleX, scaleY, 1f); // Don't upscale too much

        transform.localScale = new Vector3(scale, scale, 1f);

        // Compute top-left origin point of grid (relative to parent)
        Vector2 origin = new Vector2(
            -gridWidth / 2f + CardWidth / 2f,
             gridHeight / 2f - CardHeight / 2f
        );

        // Spawn cards
        for (int i = 0; i < totalCards; i++)
        {
            int row = i / cols;
            int col = i % cols;

            Vector3 position = new Vector3(
                origin.x + col * (CardWidth + Spacing),
                origin.y - row * (CardHeight + Spacing),
                0
            );

            Card card = Instantiate(cardPrefab, transform);
            card.transform.localPosition = position;
            card.CardId = ids[i];
        }
    }

    List<int> Shuffle(List<int> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = rng.Next(n--); // Pick a random index from 0 to n-1, then decrement n for the next iteration
            int temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }

        return list;
    }
}
