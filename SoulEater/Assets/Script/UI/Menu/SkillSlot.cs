using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlot : ButtonSlotBase
{
    [Header("Skill Related")]
    public bool isIconColorResetTrigger = false;
    public Skill skill;

    private const string skillSlotTag = "SkillSlot";

    protected override void Start()
    {
        base.Start();
        tag = skillSlotTag;
        isIconColorResetTrigger = true;
    }

    protected override void Update()
    {
        base.Update();

        // Reset Skill Icon Image Alpha.
        if (isIconColorResetTrigger && skill != null)
        {
            var newIconColor = icon.color;
            newIconColor.a = 1f;    // Set Skill Icon Opaque 1.
            icon.color = newIconColor;
            isIconColorResetTrigger = false;
        }
        else if (isIconColorResetTrigger && skill == null)
        {
            var newIconColor = icon.color;
            newIconColor.a = 0f;    // Set Skill Icon Opaque 0.
            icon.color = newIconColor;
            isIconColorResetTrigger = false;
        }
    }

    public override void OnSelect(BaseEventData e)
    {
        base.OnSelect(e);

        SkillDescription.Instance.ResetSkillDescription(skill);
    }

    public override void OnBeginDrag(PointerEventData e)
    {
        // 沒有技能則無法拖曳
        if (skill == null)
            return;
        base.OnBeginDrag(e);
    }

    public override void OnDrag(PointerEventData e)
    {
        // 沒有技能則無法拖曳
        if (skill == null)
            return;
        base.OnDrag(e);
    }

    public override void OnDrop(PointerEventData e)
    {
        SkillSlot slot = this.GetComponent<SkillSlot>();
        if (slot != null)
        {
            ChangeSlot(slotBeginDrag);
        }
        base.OnDrop(e);
    }

    public override void OnPointerClick(PointerEventData e)
    {
        base.OnPointerClick(e);
        SkillDescription.Instance.ResetSkillDescription(skill);
    }

    protected override void OnSlotClickPickUp()
    {
        // if slot doesn't have skill, return.
        if (skill == null)
            return;

        base.OnSlotClickPickUp();
    }

    public override void ChangeSlot(ButtonSlotBase sourceSlot)
    {
        SkillSlot sourceSkillSlot = sourceSlot.GetComponent<SkillSlot>();
        // Reset Icon Color.
        sourceSkillSlot.isIconColorResetTrigger = true;
        isIconColorResetTrigger = true;

        Sprite sourceIcon = sourceSkillSlot.icon.sprite;
        Skill sourceSkill = sourceSkillSlot.skill;
        sourceSkillSlot.AddSkill(this.icon.sprite, this.skill);
        AddSkill(sourceIcon, sourceSkill);

        // 若起始選取的技能欄位是快捷鍵，則同步更新戰鬥畫面上的UI技能欄
        if (slotBeginDrag.slotType == SlotType.MenuHotKey)
        {
            MenuSkillSlot menuSkillSlot = slotBeginDrag.GetComponent<MenuSkillSlot>();
            menuSkillSlot.linkSkillSlotOnCombatUI.AddSkill(sourceSlot.icon.sprite, sourceSkillSlot.skill);
            menuSkillSlot.linkSkillSlotOnCombatUI.isIconColorResetTrigger = true;
        }
    }

    public void AddSkill(Sprite icon, Skill skill)
    {
        this.icon.sprite = icon;
        this.skill = skill;
    }

    public override void RemoveSlot()
    {
        this.icon.sprite = null;
        this.skill = null;
    }
}