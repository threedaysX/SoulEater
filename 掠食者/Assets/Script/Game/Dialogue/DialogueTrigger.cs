using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueChunk dialogueChunk;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            DialogueManager.Instance.StartDialogueChunk(dialogueChunk);
        else if(Input.GetMouseButtonDown(0))
            DialogueManager.Instance.ShowNextDialogueText();

    }
}
