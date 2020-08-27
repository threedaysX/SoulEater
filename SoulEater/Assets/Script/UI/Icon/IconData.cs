using UnityEngine;
using UnityEngine.UI;

public class IconData : MonoBehaviour
{
    public bool HasData { get; private set; }
    public string IconName { get; private set; }

    public Image icon;
    public Image iconBackground;

    public void SetData(string newName, Sprite newIcon)
    {
        IconName = newName;
        icon.sprite = newIcon;
        HasData = true;
    }

    public void RemoveData()
    {
        IconName = null;
        icon.sprite = null;
        HasData = false;
    }
}
