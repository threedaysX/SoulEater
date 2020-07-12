using UnityEngine;
using UnityEngine.Events;

public class MenuEvent : MonoBehaviour
{
    public MenuLevel menuLevel;
    public UnityEvent menuEscEvent;

    public bool CheckIsSubMenu()
    {
        return menuLevel.subLevel != 1;
    }

    public bool CheckMenuLevelSame(MenuEvent newMenu)
    {
        if (newMenu.menuLevel.mainLevel == menuLevel.mainLevel &&
            newMenu.menuLevel.subLevel == menuLevel.subLevel)
        {
            return true;
        }
        return false;
    }
}

[System.Serializable]
public struct MenuLevel
{
    public int mainLevel;   // 1-1, 2-1, 3-2 => n-1 => infront dash that [n]
    public int subLevel;    // 1-1, 2-1, 3-2 => 1-n => behind dash that [n]
}
