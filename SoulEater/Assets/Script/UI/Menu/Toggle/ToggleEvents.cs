using System.Collections.Generic;
using UnityEngine;

public class ToggleEvents : MonoBehaviour
{
    public List<ToggleEvent> events;

    private void Start()
    {
        ResetToggleEvents();
    }

    /// <summary>
    /// Reset all toggle event(Include inactive).
    /// </summary>
    public void ResetToggleEvents()
    {
        foreach (ToggleEvent item in events)
        {
            item.onValueChangedEvent.Invoke(item.IsOn);
        }
    }
}
