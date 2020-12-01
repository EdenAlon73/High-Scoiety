using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManue : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject WinMenuUI;
   

    public void WinGame()
    {
        WinMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void QuitGame()
    {
        Debug.Log("quit game");
        Application.Quit();

    }

    public void MainMenu()
    {
        Debug.Log("loading main menu");
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}

