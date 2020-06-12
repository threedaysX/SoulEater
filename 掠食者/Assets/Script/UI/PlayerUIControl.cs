using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIControl : Singleton<PlayerUIControl>
{
    [Header("Hp")]
    public Image healthBar;
    public Text maxHpText;
    public Text currentHpText;

    [Header("Mana")]
    public ManaCrystal[] manas;
    public Text maxManaText;
    public Text currentManaText;
    public Sprite emptyMana;
    public Sprite fullManaLevel1;
    public Sprite fullManaLevel2;
    public Sprite fullManaLevel3;
    private int notDisplayMpSlotsCount = 0; // 未顯示的魔力水晶(小於上限時)

    [Header("SkillSlots")]
    public MenuSkillSlot[] skillUISlots;
    private Dictionary<KeyCode, MenuSkillSlot> skillSlotsDict;

    private void Start()
    {
        skillSlotsDict = new Dictionary<KeyCode, MenuSkillSlot>();
        foreach (var skillSlot in skillUISlots)
        {
            skillSlotsDict.Add(skillSlot.keyCode, skillSlot);
        }
    }

    private void Update()
    {
        StartSkillCoolDownHint();
    }

    public void SetHealthUI(float maxHealth, float currentHealth)
    {
        if (maxHpText == null || currentHpText == null)
            return;
        maxHpText.text = maxHealth.ToString();
        currentHpText.text = currentHealth.ToString();
        HealthManaControl.Instance.SetHealthBar(healthBar, maxHealth, currentHealth);
    }

    public void SetManaUI(float maxMana, float currentMana)
    {
        if (manas == null || maxManaText == null || currentManaText == null)
            return;

        int manaLimit = manas.Length;
        maxManaText.text = maxMana.ToString();
        currentManaText.text = currentMana.ToString();

        // 重置Mana上限水晶數量
        if (maxMana < manaLimit && notDisplayMpSlotsCount == 0)
        {
            for (int i = (int)maxMana; i < manaLimit; i++)
            {
                manas[i].slot.gameObject.SetActive(false);
                notDisplayMpSlotsCount++;
            }
        }

        // 重置Mana當前水晶數量
        if (currentMana >= 0)
        {
            int maxlevel = (int)currentMana / manaLimit;
            int levelSetCount = (int)currentMana % manaLimit;
            for (int i = 0; i < manaLimit; i++)
            {
                if (i < levelSetCount)
                {
                    manas[i].level = maxlevel + 1;
                }
                else
                {
                    manas[i].level = maxlevel;
                }
            }
        }

        // 重置Mana水晶UI圖示
        foreach (var mana in manas)
        {
            if (notDisplayMpSlotsCount > 0 && maxMana >= manaLimit)
            {
                for (int i = 0; i < notDisplayMpSlotsCount; i++)
                {
                    manas[manas.Length - 1 - i].slot.gameObject.SetActive(true);
                }
                notDisplayMpSlotsCount = 0;
            }
            switch (mana.level)
            {
                case 0:
                    mana.slot.sprite = emptyMana;
                    break;
                case 1:
                    mana.slot.sprite = fullManaLevel1;
                    break;
                case 2:
                    mana.slot.sprite = fullManaLevel2;
                    break;
                case 3:
                    mana.slot.sprite = fullManaLevel3;
                    break;
                default:
                    break;
            }
        }
    }

    public void StartSkillCoolDownHint()
    {
        foreach (var skillSlot in skillUISlots)
        {
            // 當季能進入冷卻，開啟冷卻提示
            if (skillSlot.skill != null && skillSlot.skill.cooling)
            {
                skillSlot.background.fillAmount = (skillSlot.skill.trueCoolDown - skillSlot.skill.coolDownTimer) / skillSlot.skill.trueCoolDown;
            }
            // 當冷卻提示不為預設值(預設為1)，且當技能不在冷卻中，或是該欄位沒有技能，則重置冷卻提示
            else if (skillSlot.background.fillAmount != 1)
            {
                if (skillSlot.skill == null ||
                   (skillSlot.skill != null && !skillSlot.skill.cooling))
                {
                    skillSlot.background.fillAmount = 1;
                }
            }
        }
    }
}

[System.Serializable]
public class ManaCrystal 
{
    public Image slot;
    public int level;
}