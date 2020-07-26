using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : Singleton<EndGameManager>
{
    public GameObject endGameUI;
    public Text endGameUIText;
    public string[] endGameUITextList;
    private int endGameTextIndex;
    private bool inEndGame;

    private void Start()
    {
        endGameTextIndex = 0;
        inEndGame = false;
        endGameUI.SetActive(false);
    }

    private void Update()
    {
        if (inEndGame)
        {
            if (Input.GetKeyDown(HotKeyController.attackKey1) ||
                Input.GetKeyDown(HotKeyController.attackKey2) ||
                Input.GetMouseButtonDown(0))
            {
                if (endGameTextIndex >= endGameUITextList.Length)
                {
                    inEndGame = false;
                    endGameUI.SetActive(false);
                    return;
                }
                endGameUIText.text = endGameUITextList[endGameTextIndex++];
            }
        }
    }

    public void EndGame()
    {
        endGameUI.SetActive(true);
        endGameUIText.text = endGameUITextList[endGameTextIndex++];
        inEndGame = true;
    }
}
