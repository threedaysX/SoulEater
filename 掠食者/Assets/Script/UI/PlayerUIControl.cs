using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIControl : Singleton<PlayerUIControl>
{
    [Header("Hp")]
    public Image healthBar;
    public Image healthBarWhite;
    public Text maxHpText;
    public Text currentHpText;

    [Header("Mana")]
    public Image manaBar;
    public Image manaBarWhite;
    public Text maxManaText;
    public Text currentManaText;

    [Header("SkillSlots")]
    public ParticleSystem slotRefreshEffect;
    public MenuSkillSlot[] skillUISlots;
    public Color32 originHintColor;
    public Color32 inCoolDownHintColor;
    private Dictionary<MenuSkillSlot, bool> skillSlotsIsDirtyDict;

    private void Start()
    {
        skillSlotsIsDirtyDict = new Dictionary<MenuSkillSlot, bool>();
        foreach (var skillSlot in skillUISlots)
        {
            skillSlotsIsDirtyDict.Add(skillSlot, true);
        }
    }

    private void Update()
    {
        StartSkillCoolDownHint();
    }

    public void SetHealthUI(float maxHealth, float currentHealth)
    {
        if (healthBar == null || maxHpText == null || currentHpText == null)
            return;

        maxHpText.text = maxHealth.ToString();
        currentHpText.text = currentHealth.ToString();
        UIImageControll.Instance.SetImageFillAmount(healthBar, healthBarWhite, maxHealth, currentHealth);
    }

    public void SetManaUI(float maxMana, float currentMana)
    {
        if (manaBar == null || maxManaText == null || currentManaText == null)
            return;

        maxManaText.text = maxMana.ToString();
        currentManaText.text = currentMana.ToString();
        UIImageControll.Instance.SetImageFillAmount(manaBar, manaBarWhite, maxMana, currentMana);
    }

    public void StartSkillCoolDownHint()
    {
        foreach (var skillSlot in skillUISlots)
        {
            // 當技能進入冷卻，開啟冷卻提示
            if (skillSlot.skill != null && skillSlot.skill.cooling)
            {
                skillSlot.background.fillAmount = (skillSlot.skill.trueCoolDown - skillSlot.skill.coolDownTimer) / skillSlot.skill.trueCoolDown;
                skillSlot.background.color = inCoolDownHintColor;
                skillSlotsIsDirtyDict[skillSlot] = true;
            }
            // 當冷卻提示不為預設值(預設為1)
            else if (skillSlotsIsDirtyDict[skillSlot])
            {
                // 且當技能不在冷卻中，或是該欄位沒有技能，則重置冷卻提示
                if (skillSlot.skill == null ||
                   (skillSlot.skill != null && !skillSlot.skill.cooling))
                {
                    skillSlotsIsDirtyDict[skillSlot] = false;
                    skillSlot.background.fillAmount = 1;
                    skillSlot.background.color = originHintColor;
                    slotRefreshEffect.transform.position = skillSlot.transform.position;
                    slotRefreshEffect.Play(true);
                }
            }
        }
    }
}
