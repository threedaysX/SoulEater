using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : Singleton<MenuControl>
{
    public GameObject menu;
    public GameObject defaultButtonEvents;
    public GameObject defaultOpenMenuContent;
    public Button defaultSelectedButton;
    public Stack<MenuEvent> menuEscStack;

    private void Start()
    {
        menu.SetActive(false);
        menuEscStack = new Stack<MenuEvent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.EscMenuKey)))
        {
            if (menu.activeSelf && menuEscStack.Count > 0)
            {
                // When User click [ESC] button.
                // Is current is sub-menu, just pop. (Because this action would not close any window.)
                // Is current is main-menu, just peek. (User can use another way to close window, like mouse click, so need to pop stack manually with menuEscEvent.
                // Ex: pop stack by [Back to previous step button click].)
                if (menuEscStack.Peek().CheckIsSubMenu())
                {
                    menuEscStack.Pop().menuEscEvent.Invoke();
                }
                else
                {
                    menuEscStack.Peek().menuEscEvent.Invoke();
                }
            }
            else
            {
                OpenMainMenu();
            }
        }   
    }

    public void PushOpenMenuToStack(MenuEvent newMenu)
    {
        // If new open menu level is same within menu stack, pop old one and push again. (Reset)
        if (menuEscStack.Count != 0 && menuEscStack.Peek().CheckMenuLevelSame(newMenu))
        {
            menuEscStack.Pop();
        }
        menuEscStack.Push(newMenu);
    }

    /// <summary>
    /// Pop until current level is the same as previous level.
    /// </summary>
    public void PopStack()
    {
        if (menuEscStack.Count == 0)
            return;

        int currentLevel = menuEscStack.Peek().menuLevel.mainLevel;
        while (menuEscStack.Count != 0 && menuEscStack.Peek().menuLevel.mainLevel == currentLevel)
        {
            menuEscStack.Pop();
        }
    }

    public void CloseMainMenu()
    {
        AppControl.MenuPausedGame(false);
        // Reset timeScale when close menu.
        TimeScaleController.Instance.OpenUI(false);

        foreach (Transform menuContent in menu.transform)
        {
            menuContent.gameObject.SetActive(false);
        }
        menu.SetActive(false);
    }

    public void OpenMainMenu()
    {
        AppControl.MenuPausedGame(true);
        TimeScaleController.Instance.OpenUI(true);

        menu.SetActive(true);
        defaultButtonEvents.SetActive(true);
        defaultOpenMenuContent.SetActive(true);
        PushOpenMenuToStack(defaultOpenMenuContent.GetComponent<MenuEvent>());

        defaultSelectedButton.Select();
        ButtonEvents.Instance.SelectButton(defaultSelectedButton);
    }
}
