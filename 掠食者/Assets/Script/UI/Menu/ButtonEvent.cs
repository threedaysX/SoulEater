using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour, ISelectHandler
{
    public GameObject pointer;
    public RectTransform targetPos;
    public UnityEvent onSelectEvent;

    public void OnSelect(BaseEventData e)
    {
        if (!pointer.activeSelf)
        {
            pointer.SetActive(true);
            return;
        }
        pointer.transform.position = targetPos.position;
        ButtonEvents.Instance.selectedButton = GetComponent<Button>();
        onSelectEvent.Invoke();
    }
}
