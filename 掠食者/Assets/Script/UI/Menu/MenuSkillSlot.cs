using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSkillSlot : SkillSlot
{
    [Header("Sync Slot(Linked Slot)")]
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
        linkSkillSlotOnCombatUI.isIconColorResetTrigger = true;

        if (slotBeginDrag != null && slotBeginDrag.slotType == SlotType.MenuHotKey)
        {
            MenuSkillSlot menuSkillSlot = slotBeginDrag.GetComponent<MenuSkillSlot>();
            var linkedSlot = menuSkillSlot.linkSkillSlotOnCombatUI;
            linkedSlot.AddSkill(menuSkillSlot.icon.sprite, menuSkillSlot.skill);
            linkedSlot.isIconColorResetTrigger = true;
        }
    }
}
