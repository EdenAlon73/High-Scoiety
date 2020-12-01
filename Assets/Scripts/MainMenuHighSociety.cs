using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHighSociety : MonoBehaviour
{

    [SerializeField] GameManager gameManager;
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        gameManager.canPlayerMove = true;
    }
    

    public void QuitGame()
    {
        Debug.Log("quit game");
        Application.Quit();

    }

   
}
