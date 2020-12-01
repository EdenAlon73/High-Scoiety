using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int timeOffRoad;
    public bool canPlayerMove = true;
    public bool playerWin = false;
    public DialogueTrigger dialogueTrigger;
    public DialogueTrigger dialogueTriggerNext;
    public DialogueTrigger dialogueTriggerFinal;
    public DialogueTrigger dialogueTriggerEndFirstLineTest;


    private void Awake()
    {
    
        timeOffRoad = 0;
        FindObjectOfType<DialogueManager>().IsLastDialogue = false;

    }

    private void Update()
    {
        TriggerTheDialogue();
        ExitGame();
    }

    public void NextLevel()
    {
        Debug.Log("You Win");
        SceneManager.LoadScene(2);
    }
    public void TriggerTheDialogue()
    {
        if (timeOffRoad == 100)
        {
            dialogueTrigger.TriggerDialogue();
        }
        else if (timeOffRoad == 400)
        {
            dialogueTriggerNext.TriggerDialogue();
        }
        else if (timeOffRoad == 1000)
            dialogueTriggerFinal.TriggerDialogue();
    }
   public void Lose()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindObjectOfType<DialogueManager>().IsLastDialogue = false;
        timeOffRoad = 0;

    }
    public void EndFirstLineTestDialogueTrigger()
    {
        dialogueTriggerEndFirstLineTest.TriggerDialogue();
    }
  void ExitGame()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
