using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public GameObject menu;
    public Button defaultSelectedButton;

    private bool pauseGame;

    private void Start()
    {
        menu.SetActive(false);
        pauseGame = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(HotKeyController.menuKey))
        {
            if (menu.activeSelf)
            {
                menu.SetActive(false);
            }
            else
            {
                menu.SetActive(true);
                defaultSelectedButton.Select();
                pauseGame = true;
                Time.timeScale = 0;
            }
        }   

        // Reset timeScale when close menu.
        if (pauseGame && !menu.activeSelf)
        {
            Time.timeScale = 1f;
            pauseGame = false;
        }
    }
}
