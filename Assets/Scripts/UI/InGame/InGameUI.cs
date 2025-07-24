using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    public TextMeshProUGUI  ScoreText;
    public void SetScoreText(int Score)
    {
        ScoreText.text = Score.ToString();
    }
    
}
