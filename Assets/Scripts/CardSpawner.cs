using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class CardSpawner : MonoBehaviour
{
    public SpriteAtlas SpriteAtlas;
    public Card CardPrefab;
    [HideInInspector]
    public int Rows = 2;
    [HideInInspector]
    public int Cols = 2;

    public float Spacing = 1f;

    float CardWidth;     
    float CardHeight;

    public float CardFlipAnimDuration;

    public int TotalCards { get; set; }
    public List<Card> SpawnedCards;

    Sprite CardHidden;

    public void GenerateBoard(SaveData Data)
    {
        CenterSpawner();

        SpriteRenderer SR = CardPrefab.GetComponent<SpriteRenderer>();
        CardWidth = SR.bounds.size.x;
        CardHeight = SR.bounds.size.y;

        GenerateCards(Data);

    }

    void CenterSpawner()
    {
        Vector3 CamPos = Camera.main.transform.position;
        transform.position = new Vector3(CamPos.x, CamPos.y, 0f);
    }

    void GenerateCards(SaveData Data) 
    {
        Sprite[] AllSprites = new Sprite[SpriteAtlas.spriteCount];
        SpriteAtlas.GetSprites(AllSprites);
        CardHidden = AllSprites.FirstOrDefault(s => s.name.Contains("CardHidden"));

        if (Data == null)
        {
            if (GridSelectMenu.CurrentGridSelected == GridSelection.TwoCrossTwo) { Rows = 2; Cols = 2; }
            if (GridSelectMenu.CurrentGridSelected == GridSelection.TwoCrossThree) { Rows = 2; Cols = 3; }
            if (GridSelectMenu.CurrentGridSelected == GridSelection.FiveCrossSix) { Rows = 5; Cols = 6; }

            TotalCards = Rows * Cols;

            Sprite[] CardShownSprites = AllSprites.Where(s => !s.name.Contains("CardHidden")).ToArray();

            List<Sprite> Cards = new List<Sprite>();
            for (int i = 0; i < TotalCards / 2; i++)
            {
                Sprite Sprite = CardShownSprites[Random.Range(0, CardShownSprites.Length)];
                Cards.Add(Sprite);
                Cards.Add(Sprite);
            }

            // Shuffle cards
            Cards = Cards.OrderBy(x => Random.value).ToList();

            PlaceCardsOnBoard(Cards);
        }
        else 
        {
            foreach (var item in Data.CardsState)
            {
                Card Card = Instantiate(CardPrefab, transform);
                Card.transform.localPosition = item.CardLocation;
                Card.CardHidden = CardHidden;
                Card.CardShown = AllSprites.FirstOrDefault(s => s.name.Contains(item.CardShownName));
                Card.CardFlipAnimDuration = CardFlipAnimDuration;

                SpawnedCards.Add(Card);
            }
            transform.localScale = new Vector3(Data.CardSpawnerScaleX, Data.CardSpawnerScaleY, 1);
        }
    }

    void PlaceCardsOnBoard(List<Sprite> Cards)
    {
        // Compute total grid size
        float GridWidth = Cols * (CardWidth + Spacing) - Spacing;
        float GridHeight = Rows * (CardHeight + Spacing) - Spacing;

        // Get screen dimensions in world space
        float ScreenHeight = Camera.main.orthographicSize * 2f;
        float ScreenWidth = ScreenHeight * Camera.main.aspect;

        float MaxWidth = ScreenWidth * 0.8f;
        float MaxHeight = ScreenHeight * 0.8f;

        // Calculate scale factor to fit within screen
        float ScaleX = MaxWidth / GridWidth;
        float ScaleY = MaxHeight / GridHeight;
        float Scale = Mathf.Min(ScaleX, ScaleY, 1f);

        transform.localScale = new Vector3(Scale, Scale, 1f);

        // Compute top-left origin point of grid (relative to parent)
        Vector2 Origin = new Vector2(
            -GridWidth / 2f + CardWidth / 2f,
             GridHeight / 2f - CardHeight / 2f
        );

        // Spawn cards
        for (int i = 0; i < TotalCards; i++)
        {
            int Row = i / Cols;
            int Col = i % Cols;

            Vector3 Position = GetCardPosition(Row, Col, Origin);

            Card Card = Instantiate(CardPrefab, transform);
            Card.transform.localPosition = Position;
            Card.CardHidden = CardHidden;
            Card.CardShown = Cards[i];
            Card.CardFlipAnimDuration = CardFlipAnimDuration;
            SpawnedCards.Add(Card);
        }
    }

    private Vector3 GetCardPosition(int Row, int Col, Vector2 Origin)
    {
        return new Vector3(
            Origin.x + Col * (CardWidth + Spacing),
            Origin.y - Row * (CardHeight + Spacing),
            0
        );
    }
}
