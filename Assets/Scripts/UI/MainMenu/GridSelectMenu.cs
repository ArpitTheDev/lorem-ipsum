using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GridSelection { TwoCrossTwo, TwoCrossThree, FiveCrossSix }

public class GridSelectMenu : MonoBehaviour
{
    public MainMenu MainMenu;
    public static GridSelection CurrentGridSelected;

    public void OnTwoCrossTwoButtonCliked()
    {
        CurrentGridSelected = GridSelection.TwoCrossTwo;
    }

    public void OnTwoCrossThreeButtonCliked()
    {
        CurrentGridSelected = GridSelection.TwoCrossThree;
    }

    public void OnFiveCrossSixButtonCliked()
    {
        CurrentGridSelected = GridSelection.FiveCrossSix;
    }

    public void OnExitButtonCliked()
    {
        MainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

}
