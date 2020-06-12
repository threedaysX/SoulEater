using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvents : Singleton<ButtonEvents>
{
    public Button selectedButton;
    public AudioClip selectButtonSound;
    private bool deselectButtonTrigger;

    public void Update()
    {
        if (selectedButton != null && selectedButton.GetComponent<ButtonEvent>().canPressAnyKey
            && Input.anyKeyDown)
        {
            selectedButton.onClick.Invoke();
        }
        else if (Input.GetKeyDown(HotKeyController.attackKey1))
        {
            selectedButton.onClick.Invoke();
        }

        // If lost select, then click any key to re-select last button (except mouse click).
        if (deselectButtonTrigger && Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            selectedButton.Select();
            deselectButtonTrigger = false;
        }
    }

    public void SelectButton(Button button)
    {
        selectedButton = button;
    }

    public void DeselectButton()
    {
        if (!deselectButtonTrigger)
            deselectButtonTrigger = true;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void DelayLoadScene(string sceneName)
    {
        StartCoroutine(DelayLoadScene(sceneName, 2.5f));
    }

    private IEnumerator DelayLoadScene(string sceneName, float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGameImmediately()
    {
        Application.Quit();
    }

    public void QuitGame()
    {
        StartCoroutine(DelayQuitGame(1f));
    }

    private IEnumerator DelayQuitGame(float duration)
    {
        yield return new WaitForSeconds(duration);
        Application.Quit();
    }
}
