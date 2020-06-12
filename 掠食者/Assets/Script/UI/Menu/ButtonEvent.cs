using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool canPressAnyKey;
    public GameObject pointer;
    public RectTransform targetPos;
    public UnityEvent onSelectEvent;
    public UnityEvent onDeselectEvent;

    public void OnSelect(BaseEventData e)
    {
        if (pointer != null && !pointer.activeSelf)
        {
            pointer.SetActive(true);
            return;
        }
        if (targetPos != null)
        {
            pointer.transform.position = targetPos.position;
        }
        ButtonEvents.Instance.selectedButton = GetComponent<Button>();
        AudioControl.Instance.PlaySound(ButtonEvents.Instance.selectButtonSound);
        onSelectEvent.Invoke();
    }

    public void OnDeselect(BaseEventData e)
    {
        onDeselectEvent.Invoke();
        ButtonEvents.Instance.DeselectButton();
    }
}
