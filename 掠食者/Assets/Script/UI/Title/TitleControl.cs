using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleControl : MonoBehaviour
{
    public GameObject options;
    public Button defaultTitleSelectedButton;
    public Stack<MenuEvent> menuEscStack;

    // Start is called before the first frame update
    private void Start()
    {
        defaultTitleSelectedButton.Select();
        ButtonEvents.Instance.SelectButton(defaultTitleSelectedButton);
        options.SetActive(false);
        menuEscStack = new Stack<MenuEvent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(HotKeyController.menuKey))
        {
            if (options.activeSelf)
            {
                menuEscStack.Pop().escButton.onClick.Invoke();
            }
        }
    }

    public void PushOpenMenuToStack(MenuEvent newMenu)
    {
        menuEscStack.Push(newMenu);
    }
}