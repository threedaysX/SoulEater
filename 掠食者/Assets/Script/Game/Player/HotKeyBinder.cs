using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HotKeyBinder : MonoBehaviour
{
    [Header("Which Key on Keyboard")]
    public HotKey representHotKey;

    public Text displayKeyText;

    private bool canChangeHotKey;

    private void Start()
    {
        representHotKey.key = HotKeyController.GetHotKey(representHotKey.type);
        canChangeHotKey = false;
        ResetIfKeyDirty(true);
        GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(PopUpDialogToChangeHotKey()); });
    }

    private void Update()
    {
        if (canChangeHotKey)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kc in HotKeyController.allKeyCodes)
                {
                    // Get which user key down
                    if (Input.GetKeyDown(kc))
                    {
                        representHotKey.key = kc;
                        representHotKey.isKeyDirty = true;
                        HotKeyController.Instance.ModifyDictKey(representHotKey);
                        HotKeyController.Instance.hotkeyHintDialog.SetActive(false);
                        canChangeHotKey = false;
                    }
                }
            }
        }
        ResetIfKeyDirty();
    }

    /// <summary>
    /// Button Click To Trigger. Open dialog to hint user can start changing hot key.
    /// </summary>
    public IEnumerator PopUpDialogToChangeHotKey()
    {
        HotKeyController.Instance.hotkeyHintDialog.SetActive(true);
        yield return new WaitForEndOfFrame();
        canChangeHotKey = true;
    }

    public void ResetIfKeyDirty(bool forceReset = false)
    {
        if (representHotKey.isKeyDirty || forceReset)
        {
            displayKeyText.text = representHotKey.key.ToString(); // Reset Display Text.
            representHotKey.isKeyDirty = false;
        }
        else if (!representHotKey.isKeyDirty && representHotKey.key != HotKeyController.GetHotKey(representHotKey.type))
        {
            representHotKey.key = HotKeyController.GetHotKey(representHotKey.type); // Reset Key (Basically happened with two keys changed).
            displayKeyText.text = representHotKey.key.ToString(); // Reset Display Text.
        }
    }
}
