using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { newGame,OldGame }

public class MainMenu : MonoBehaviour
{
    public static GameState CurrentGameStateSelected;

    public GridSelectMenu GridMenu;

    public Button ContinueButton;

    private void Start()
    {
        if(SaveManager.IsSaveDataExists())
            ContinueButton.gameObject.SetActive(true);
    }

    public void OnContinueButtonCliked()
    {
        CurrentGameStateSelected = GameState.OldGame;
        SceneManager.LoadScene("Game");
    }

    public void OnNewGameButtonCliked()
    {
        CurrentGameStateSelected = GameState.newGame;
        SceneManager.LoadScene("Game");
    }

    public void OnGridSelectButtonCliked()
    {
        GridMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnQuitButtonCliked()
    {
        Application.Quit();
    }
}
