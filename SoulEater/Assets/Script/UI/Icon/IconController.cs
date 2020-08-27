using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    [Header("負面狀態槽")]
    public Transform debuffSlots;

    private IconData[] icons;
    private int currentIndex = 0;

    public Dictionary<string, IconData> CurrentExistDebuffs { get; private set; }

    private void Start()
    {
        CurrentExistDebuffs = new Dictionary<string, IconData>();
        icons = debuffSlots.GetComponentsInChildren<IconData>(true);
        foreach (IconData icon in icons)
        {
            if (icon.HasData)
            {
                CurrentExistDebuffs.Add(icon.IconName, icon);
                icon.gameObject.SetActive(true);
            }
            else
            {
                icon.gameObject.SetActive(false);
            }
        }
    }

    // Set debuff image to slot
    public void SetIcon(string name, Sprite icon)
    {
        if (CurrentExistDebuffs.ContainsKey(name))
            return;

        IconData data = icons[currentIndex];
        data.gameObject.SetActive(true);
        data.SetData(name, icon);
        CurrentExistDebuffs.Add(name, data);
        currentIndex++;
    }

    public void RemoveIcon(string name)
    {
        if (CurrentExistDebuffs.ContainsKey(name))
        {
            CurrentExistDebuffs[name].RemoveData();
            CurrentExistDebuffs[name].gameObject.SetActive(false);
            CurrentExistDebuffs.Remove(name);
            currentIndex--;
        }
    }
}
