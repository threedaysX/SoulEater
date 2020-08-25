using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleEvent : MonoBehaviour
{
    public Toggle toggle;
    [Header("Toggle IsOn 代表反義")]
    public bool isReverseMean = false;
    public bool IsOn
    {
        get
        {
            _isOn = GetToggleChecked();
            return _isOn;
        }
    }
    [SerializeField] private bool _isOn;
    public ToggleOnValueChangedEvent onValueChangedEvent;

    private void Start()
    {
        onValueChangedEvent.Invoke(IsOn);
        toggle.onValueChanged.AddListener(delegate { OnValueChangedEventCall(IsOn); });
    }

    public bool GetToggleChecked()
    {
        if (isReverseMean)
        {
            return !toggle.isOn;
        }
        else
        {
            return toggle.isOn;
        }
    }

    public void OnValueChangedEventCall(bool isChecked)
    {
        onValueChangedEvent.Invoke(isChecked);
    }

    [System.Serializable]
    public class ToggleOnValueChangedEvent : UnityEvent<bool> { }
}
