using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvents : Singleton<ButtonEvents>
{
    public Button selectedButton;

    public void Update()
    {
        if (Input.GetKeyDown(HotKeyController.attackKey1))
        {
            selectedButton.onClick.Invoke();
        }
    }

    public void SelectButton(Button button)
    {
        selectedButton = button;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
