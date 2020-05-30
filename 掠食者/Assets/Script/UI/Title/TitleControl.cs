using UnityEngine;
using UnityEngine.UI;

public class TitleControl : MonoBehaviour
{
    public GameObject options;
    public Button defaultSelectedButton;

    // Start is called before the first frame update
    private void Start()
    {
        defaultSelectedButton.Select();
        ButtonEvents.Instance.SelectButton(defaultSelectedButton);
        options.SetActive(false);
    }
}
