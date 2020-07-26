using UnityEngine.EventSystems;

public class MenuSkillSlot : SkillSlot
{
    public SkillSlot linkSkillSlotOnCombatUi;

    public override void OnDrop(PointerEventData e)
    {
        base.OnDrop(e);

        // 連結Menu與戰鬥畫面UI的技能快捷鍵
        if (linkSkillSlotOnCombatUi == null)
            return;
        linkSkillSlotOnCombatUi.AddSkill(this.icon.sprite, this.skill);
        if (skillSlotBeginDrag.slotType == SlotType.MenuHotKey)
        {
            var targetLinkSlot = skillSlotBeginDrag.GetComponent<MenuSkillSlot>().linkSkillSlotOnCombatUi;
            targetLinkSlot.AddSkill(skillSlotBeginDrag.icon.sprite, skillSlotBeginDrag.skill);
        }
    }
}
