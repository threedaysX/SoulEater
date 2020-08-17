using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    private Dictionary<int, Queue<string>> dialogueDictionary = new Dictionary<int, Queue<string>>();  //chunk編號和數量
    private int index;  //chunk 數量

    [SerializeField] private Text dialogueDisplay;
    [SerializeField] private Button[] dialogueChoiceButton;
    private Queue<string> dialogueText = new Queue<string>();


    public void StartDialogue(DialogueChunk[] dialogueChunk)
    {
        foreach(DialogueChunk chunk in dialogueChunk)
        {
            
        }
    }

    private void StartDialogueChunk(DialogueChunk dialogueChunk)
    {
        if (dialogueText == null)
            return;

        dialogueText.Clear();
        foreach (string sentence in dialogueChunk.stenences)
        {
            dialogueText.Enqueue(sentence);
        }

        if (dialogueChunk.choices > 0)
            DisplayChooseButton(dialogueChoiceButton, dialogueChunk.choices);
    }

    private void DisplayChooseButton(Button[] ChoiceButton, int buttonAmount)
    {
        foreach(Button btn in ChoiceButton)
        {
            //change button width
            btn.interactable = true;
        }
    }

    public void ShowNextDialogue()
    {
        if (dialogueText.Count == 0)
        {
            EndDialogue();
        }

        string sentence = dialogueText.Dequeue();
        dialogueDisplay.text = sentence;
    }

    /// /// /// ///

    public void JumpToDailogueChunk(DialogueChunk[] dialogueChunks, int index)
    {
        if (dialogueDictionary == null)
            return;
        this.index = 0;
        dialogueDictionary.Clear();
        Queue<string> s = new Queue<string>();
        foreach (string sentence in dialogueChunks[index].stenences)
        {
            s.Enqueue(sentence);
        }
        dialogueDictionary.Add(index, s);

        ShowNextDialogue();
    }

    private void EndDialogue()
    {
        Debug.Log("OWO");
        index = 0;
    }
}
