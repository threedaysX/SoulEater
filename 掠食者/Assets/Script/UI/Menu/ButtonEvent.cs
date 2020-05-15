using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour, ISelectHandler
{
    public GameObject pointer;
    public RectTransform targetPos;
    public UnityEvent clickEvent;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    public void OnSelect(BaseEventData e)
    {
        if (!pointer.activeSelf)
        {
            pointer.SetActive(true);
            return;
        }
        pointer.transform.position = targetPos.position;
        ButtonEvents.selectedButton = GetComponent<Button>();
    }

    public void OnClick()
    {
        clickEvent.Invoke();
    }
}
