using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    Queue<string> sentences;
    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueWindow;
  
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        GameControl.control.gameState = GameState.CUTSCENE;

        dialogueWindow.SetActive(true);

        nameText.text = dialogue.nameText;

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        //consider adding animation from Brackeys dialogue tutorial to text
    }

    void EndDialogue()
    {
        dialogueWindow.SetActive(false);
        GameControl.control.gameState = GameState.OVERWORLD;
        Time.timeScale = 1f;

    }
}
