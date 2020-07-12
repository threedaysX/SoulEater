using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotKeyController : Singleton<HotKeyController>
{
    private static Dictionary<HotKeyType, KeyCode> defaultHotKeyDict;
    public static Dictionary<HotKeyType, KeyCode> hotKeyDict;
    public static KeyCode[] allKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

    [Header("HotKey UI PlugIn")]
    public GameObject hotkeyHintDialog;

    [Header("Skill Slot PlugIn")]
    public Text[] skillSlotText1;
    public Text[] skillSlotText2;
    public Text[] skillSlotText3;
    public Text[] skillSlotText4;

    public void Start()
    {
        defaultHotKeyDict = new Dictionary<HotKeyType, KeyCode>
        {
            { HotKeyType.MoveUp, KeyCode.UpArrow },
            { HotKeyType.MoveDown, KeyCode.DownArrow },
            { HotKeyType.MoveLeft, KeyCode.LeftArrow },
            { HotKeyType.MoveRight, KeyCode.RightArrow },
            { HotKeyType.AttackKey1, KeyCode.Z },
            { HotKeyType.AttackKey2, KeyCode.X },
            { HotKeyType.JumpKey, KeyCode.Space },
            { HotKeyType.CollectKey, KeyCode.C },
            { HotKeyType.EvadeKey, KeyCode.LeftShift },
            { HotKeyType.SkillKey1, KeyCode.A },
            { HotKeyType.SkillKey2, KeyCode.S },
            { HotKeyType.SkillKey3, KeyCode.D },
            { HotKeyType.SkillKey4, KeyCode.F },
            { HotKeyType.SkillKey5, KeyCode.Q },
            { HotKeyType.SkillKey6, KeyCode.W },
            { HotKeyType.SkillKey7, KeyCode.E },
            { HotKeyType.SkillKey8, KeyCode.R },
            { HotKeyType.EscMenuKey, KeyCode.Escape },
            { HotKeyType.OpenMapKey, KeyCode.Tab }
        };
        ResetDefaultHotKey();
        hotkeyHintDialog.SetActive(false);
    }

    public static KeyCode GetHotKey(HotKeyType type)
    {
        return hotKeyDict[type];
    }

    public void ModifyDictKey(HotKey newhotKey)
    {
        CheckKeyDuplicateInDictionary(hotKeyDict[newhotKey.type], newhotKey.type, newhotKey.key);
        hotKeyDict[newhotKey.type] = newhotKey.key;
        ResetSkillSlotKeyText(newhotKey.type);
    }

    public void CheckKeyDuplicateInDictionary(KeyCode oldKeyCode, HotKeyType typeOfNewKeyCode, KeyCode newKeyCode)
    {
        foreach (var hotKey in hotKeyDict)
        {
            // If new keyCode duplicate with other type (not same type of itself).
            if (newKeyCode == hotKey.Value && typeOfNewKeyCode != hotKey.Key)
            {
                // Change KeyCode
                hotKeyDict[hotKey.Key] = oldKeyCode;
                ResetSkillSlotKeyText(hotKey.Key);
                return;
            }
        }
    }

    public void ResetDefaultHotKey()
    {
        hotKeyDict = new Dictionary<HotKeyType, KeyCode>(defaultHotKeyDict);
    }

    public void ResetSkillSlotKeyText(HotKeyType hotKeyType)
    {
        switch (hotKeyType)
        {
            case HotKeyType.SkillKey1:
                foreach (var slotText in skillSlotText1)
                {
                    slotText.text = GetHotKey(HotKeyType.SkillKey1).ToString();
                }
                break;
            case HotKeyType.SkillKey2:
                foreach (var slotText in skillSlotText2)
                {
                    slotText.text = GetHotKey(HotKeyType.SkillKey2).ToString();
                }
                break;
            case HotKeyType.SkillKey3:
                foreach (var slotText in skillSlotText3)
                {
                    slotText.text = GetHotKey(HotKeyType.SkillKey3).ToString();
                }
                break;
            case HotKeyType.SkillKey4:
                foreach (var slotText in skillSlotText4)
                {
                    slotText.text = GetHotKey(HotKeyType.SkillKey4).ToString();
                }
                break;
        }
    }
}

[System.Serializable]
public struct HotKey
{
    public bool isKeyDirty;
    public HotKeyType type;
    public KeyCode key;
}

public enum HotKeyType
{
    MoveRight,
    MoveLeft,
    MoveUp,
    MoveDown,
    AttackKey1,
    AttackKey2,
    JumpKey,
    CollectKey,
    EvadeKey,
    SkillKey1,
    SkillKey2,
    SkillKey3,
    SkillKey4,
    SkillKey5,
    SkillKey6,
    SkillKey7,
    SkillKey8,
    EscMenuKey,
    OpenMapKey,
}
