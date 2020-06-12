using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSkillSlot : SkillSlot
{
    public KeyCode keyCode;
    public SkillSlot linkSkillSlotOnCombatUI;

    public override void OnSlotClick()
    {
        base.OnSlotClick();
        ResetPlayerCombatSkillSlotUI();
    }

    public override void OnDrop(PointerEventData e)
    {
        base.OnDrop(e);
        ResetPlayerCombatSkillSlotUI();
    }

    private void ResetPlayerCombatSkillSlotUI()
    {
        // 連結Menu與戰鬥畫面UI的技能快捷鍵
        if (linkSkillSlotOnCombatUI == null)
            return;
        linkSkillSlotOnCombatUI.AddSkill(this.icon.sprite, this.skill);
        if (skillSlotBeginDrag.slotType == SlotType.MenuHotKey)
        {
            var targetLinkSlot = skillSlotBeginDrag.GetComponent<MenuSkillSlot>().linkSkillSlotOnCombatUI;
            targetLinkSlot.AddSkill(skillSlotBeginDrag.icon.sprite, skillSlotBeginDrag.skill);
        }
    }
}
