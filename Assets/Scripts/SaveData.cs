using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int Score;
    public int MatchCounter;
    public int MatchMultiplierCounter;
    public int Rows;
    public int Cols;
    public float CardSpawnerScaleX;
    public float CardSpawnerScaleY;
    public List<CardData> CardsState;
}

[System.Serializable]
public class CardData
{
    public string CardShownName;
    public Vector3 CardLocation;
}