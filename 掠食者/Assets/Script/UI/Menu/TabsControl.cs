using UnityEngine.UI;

public class TabsControl : Singleton<TabsControl>
{
    public Button[] tabs;
    private bool isActivedTabs = false;

    public void Update()
    {
        if (isActivedTabs && ButtonSlotBase.GetDragingState())
        {
            isActivedTabs = false;
            SetTabsButtonInteractive(false);
        }
        else if (!isActivedTabs && !ButtonSlotBase.GetDragingState())
        {
            isActivedTabs = true;
            SetTabsButtonInteractive(true);
        }
    }

    public void SetTabsButtonInteractive(bool active)
    {
        foreach (var tab in tabs)
        {
            tab.interactable = active;
        }
    }
}
