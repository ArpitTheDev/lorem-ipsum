using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class CardSpawner : MonoBehaviour
{
    public SpriteAtlas spriteAtlas;
    public Card cardPrefab;

    public int rows = 2;
    public int cols = 2;

    public float Spacing = 2f;

    float CardWidth;     
    float CardHeight;

    public float CardFlipAnimDuration;
    int totalCards;
    public int TotalCards => totalCards;
    public List<Card> SpawnedCards;

    Sprite CardHidden;

    public void GenerateBoard(SaveData data)
    {
        CenterSpawner();

        SpriteRenderer sr = cardPrefab.GetComponent<SpriteRenderer>();
        CardWidth = sr.bounds.size.x;
        CardHeight = sr.bounds.size.y;

        GenerateCards(data);

    }

    void CenterSpawner()
    {
        Vector3 camPos = Camera.main.transform.position;
        transform.position = new Vector3(camPos.x, camPos.y, 0f);
    }

    void GenerateCards(SaveData data) 
    {
        Sprite[] allSprites = new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(allSprites);
        CardHidden = allSprites.FirstOrDefault(s => s.name.Contains("CardHidden"));

        if (data == null)
        {
            totalCards = rows * cols;

            Sprite[] CardShownSprites = allSprites.Where(s => !s.name.Contains("CardHidden")).ToArray();

            List<Sprite> cards = new List<Sprite>();
            for (int i = 0; i < totalCards / 2; i++)
            {
                Sprite sprite = CardShownSprites[Random.Range(0, CardShownSprites.Length)];
                cards.Add(sprite);
                cards.Add(sprite);
            }

            // Shuffle cards
            cards = cards.OrderBy(x => Random.value).ToList();

            PlaceCardsOnBoard(cards);
        }
        else 
        {
            foreach (var item in data.CardsState)
            {
                Card card = Instantiate(cardPrefab, transform);
                card.transform.localPosition = item.CardLocation;
                card.CardHidden = CardHidden;
                card.CardShown = allSprites.FirstOrDefault(s => s.name.Contains(item.CardShownName));
                card.CardFlipAnimDuration = CardFlipAnimDuration;

                SpawnedCards.Add(card);
            }
            transform.localScale = new Vector3(data.CardSpawnerScaleX, data.CardSpawnerScaleY, 1);
        }
    }

    void PlaceCardsOnBoard(List<Sprite> cards)
    {
       

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
            card.CardHidden = CardHidden;
            card.CardShown = cards[i];
            card.CardFlipAnimDuration = CardFlipAnimDuration;
            SpawnedCards.Add(card);
        }
    }
}
