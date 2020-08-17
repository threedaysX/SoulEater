using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueChunk[] dialogueChunk;

    private void Start()
    {
        DialogueManager.Instance.StartDialogue(dialogueChunk);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            DialogueManager.Instance.ShowNextDialogue();

        if (Input.GetKeyDown(KeyCode.O))
            DialogueManager.Instance.JumpToDailogueChunk(dialogueChunk, 1);
    }
}
