using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private IEnumerator typeSentenceCo = null;
    
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    private Queue<string> sentences;
    public bool IsLastDialogue = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
            
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        IsLastDialogue = dialogue.LastDialogue;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
          sentences.Enqueue(sentence);   
            
        }

         DisplayNextSentence();
        
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        string sentence = sentences.Dequeue();
        if (typeSentenceCo != null)
            StopCoroutine(typeSentenceCo);

        typeSentenceCo = TypeSentenceCoroutine(sentence);
        
        StartCoroutine(typeSentenceCo);
        

    }

   
    private IEnumerator TypeSentenceCoroutine(string sentence)

    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        typeSentenceCo = null;
    }


public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        if (IsLastDialogue == true) FindObjectOfType<GameManager>().Lose();
        
    }
}
