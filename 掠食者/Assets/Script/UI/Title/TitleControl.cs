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
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.EscMenuKey)))
        {
            if (options.activeSelf)
            {
                menuEscStack.Pop().menuEscEvent.Invoke();
            }
        }
    }

    public void PushOpenMenuToStack(MenuEvent newMenu)
    {
        menuEscStack.Push(newMenu);
    }
}