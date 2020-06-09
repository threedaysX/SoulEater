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
        if (Input.GetKeyDown(HotKeyController.menuKey))
        {
            if (menu.activeSelf)
            {
                menuEscStack.Pop().escButton.onClick.Invoke();
            }
            else
            {
                OpenMainMenu();
            }
        }   
    }

    public void PushOpenMenuToStack(MenuEvent newMenu)
    {
        menuEscStack.Push(newMenu);
    }

    public void CloseMainMenu()
    {
        foreach (Transform menuContent in menu.transform)
        {
            menuContent.gameObject.SetActive(false);
        }
        menu.SetActive(false);
        // Reset timeScale when close menu.
        TimeScaleController.Instance.OpenUI(false);
    }

    public void OpenMainMenu()
    {
        menu.SetActive(true);
        defaultButtonEvents.SetActive(true);
        defaultOpenMenuContent.SetActive(true);
        PushOpenMenuToStack(defaultOpenMenuContent.GetComponent<MenuEvent>());
        TimeScaleController.Instance.OpenUI(true);

        if (ButtonEvents.Instance.selectedButton != null)
        {
            ButtonEvents.Instance.selectedButton.Select();
            return;
        }
        defaultSelectedButton.Select();
    }
}
