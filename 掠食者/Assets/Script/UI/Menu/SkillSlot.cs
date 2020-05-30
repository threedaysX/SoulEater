using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public Image icon;
    public Skill skill;
    public SlotType slotType;

    protected static SkillSlot skillSlotBeginDrag;
    private const string skillSlotTag = "SkillSlot";
    private Canvas slotCanvasSetting;
    private int originSortOrder;

    private void Start()
    {
        tag = skillSlotTag;
        slotCanvasSetting = icon.GetComponent<Canvas>();
        originSortOrder = slotCanvasSetting.sortingOrder;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        // 沒有技能則無法拖曳
        if (skill == null)
            return;
        SkillDescription.Instance.SetSkillTitleName(skill.skillName);
        SkillDescription.Instance.SetSkillDescription(skill.description);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        skillSlotBeginDrag = this;

        slotCanvasSetting.overrideSorting = true;
        slotCanvasSetting.sortingOrder = 10;
    }

    public void OnDrag(PointerEventData e)
    {
        // 沒有技能則無法拖曳
        if (skill == null)
            return;
        icon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData e)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        icon.transform.localPosition = Vector3.zero;
        slotCanvasSetting.overrideSorting = false;
        slotCanvasSetting.sortingOrder = originSortOrder;
    }

    public virtual void OnDrop(PointerEventData e)
    {
        SkillSlot slot = this.GetComponent<SkillSlot>();
        if (slot != null)
        {
            ChangeSkillSlot(skillSlotBeginDrag);
        }
    }

    public virtual void OnPointerClick(PointerEventData e) 
    {
        if (skill == null)
            return;

        SkillDescription.Instance.SetSkillTitleName(skill.skillName);
        SkillDescription.Instance.SetSkillDescription(skill.description);
    }

    public void AddSkill(Sprite icon, Skill skill)
    {
        this.icon.sprite = icon;
        this.skill = skill;
    }

    public void RemoveSkill()
    {
        this.icon.sprite = null;
        this.skill = null;
    }

    public void ChangeSkillSlot(SkillSlot sourceSlot)
    {
        Sprite sourceIcon = sourceSlot.icon.sprite;
        Skill sourceSkill = sourceSlot.skill;
        sourceSlot.AddSkill(this.icon.sprite, this.skill);
        AddSkill(sourceIcon, sourceSkill);

        // 若交換的技能欄位是快捷鍵，則更新戰鬥畫面上的UI技能欄
        if (skillSlotBeginDrag.slotType == SlotType.MenuHotKey)
        {
            skillSlotBeginDrag.GetComponent<MenuSkillSlot>().linkSkillSlotOnCombatUi.AddSkill(sourceSlot.icon.sprite, sourceSlot.skill);
        }
    }
}

public enum SlotType
{
    Inventory,
    MenuHotKey
}