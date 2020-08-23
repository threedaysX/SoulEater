using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject dialogueDisplay;
    [SerializeField] private Text dialogueDisplayContent;
    [SerializeField] private Button[] dialogueChoiceButton;
    private Queue<string> dialogueText = new Queue<string>();
    private DialogueChunk currentDialogueChunk;
    private bool isDialoguing = false;

    private void Start()
    {
        foreach (Button btn in dialogueChoiceButton)
            btn.GetComponent<Button>();
    }

    private void Update()
    {
        //if(isDialoguing)
            //player.move.canDo = isDialoguing ? false : true;  //
            //player.attack.canDo = isDialoguing ? false : true;  //
    }

    public void StartDialogueChunk(DialogueChunk dialogueChunk)
    {
        if (dialogueText == null)
            return;

        isDialoguing = true;
        dialogueDisplay.gameObject.SetActive(true);
        currentDialogueChunk = dialogueChunk;
        dialogueText.Clear();
        foreach (string sentence in dialogueChunk.stenences)
        {
            dialogueText.Enqueue(sentence);
        }
        
        ShowNextDialogueText();
    }


    public void ShowNextDialogueText()
    {
        if (currentDialogueChunk.bifurcationAmount > 0 && dialogueText.Count == 0)
        {
            ShowChooseButton(dialogueChoiceButton, currentDialogueChunk.bifurcationAmount);
            return;
        }
        else if (currentDialogueChunk.bifurcationAmount == 0 && dialogueText.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogueText.Dequeue();
        dialogueDisplayContent.text = sentence;
    }

    private void ShowChooseButton(Button[] ChoiceButton, int buttonAmount)
    {
        if (buttonAmount <= 0)
            return;

        for (int i = 0; i < buttonAmount; i++)
        {
            //change button width
            int width = (550 - 50 * i) / buttonAmount;
            int pos = (width + 50) * i;

            ChoiceButton[i].GetComponent<RectTransform>().sizeDelta = new Vector2(width, 30);
            ChoiceButton[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos, default);

            if (i == 0 && ChoiceButton[i].onClick != null)  //如果跑第一個按鈕前面還有onClick事件則清除全部方法
                ChoiceButton[i].onClick.RemoveAllListeners();

            ChoiceButton[i].gameObject.SetActive(true);
            ChoiceButton[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = currentDialogueChunk.bifurcationChunk[i].chunkName;
            AddListener(ChoiceButton[i], i);
        }
    }

    private void StartPotentialChunk(int btnIndex)
    {
        Debug.Log(btnIndex);
        foreach(Button btn in dialogueChoiceButton)
        {
            btn.gameObject.SetActive(false);
        }
       StartDialogueChunk(currentDialogueChunk.bifurcationChunk[btnIndex]);
    }

    private void EndDialogue()
    {
        isDialoguing = false;
        currentDialogueChunk = null;
        dialogueDisplay.gameObject.SetActive(false);
    }

    private void AddListener(Button btn, int btnIndex)
    {
        btn.onClick.AddListener(() => StartPotentialChunk(btnIndex));
    }

    /// /// /// ///

    //public void jumptodailoguechunk(dialoguechunk[] dialoguechunks, int index)
    //{
    //    if (dialoguedictionary == null)
    //        return;
    //    this.index = 0;
    //    dialoguedictionary.clear();
    //    queue<string> s = new queue<string>();
    //    foreach (string sentence in dialoguechunks[index].stenences)
    //    {
    //        s.enqueue(sentence);
    //    }
    //    dialoguedictionary.add(index, s);

    //    shownextdialoguechunk();
    //}
}
