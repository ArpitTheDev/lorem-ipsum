using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : InGameUI
{
    public void OnGameOverButtonClicked()
    {
        SceneManager.LoadScene("Menu");
    }
}
