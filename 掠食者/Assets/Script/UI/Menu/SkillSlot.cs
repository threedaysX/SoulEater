using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public Image background;
    public Image icon;
    public Skill skill;
    public SlotType slotType;

    protected static SkillSlot skillSlotBeginDrag;
    private static bool isClickDrag;
    private static Vector3 lastMousePos;
    private const string skillSlotTag = "SkillSlot";
    private int originSortOrder;
    private Canvas slotCanvasSetting;
    private Color originalBackgroundColor;
    private Color selectedBackgroundColor;

    private void Awake()
    {
        background = GetComponent<Image>();
        slotCanvasSetting = icon.GetComponent<Canvas>();
    }

    protected virtual void Start()
    {
        tag = skillSlotTag;
        originSortOrder = slotCanvasSetting.sortingOrder;
        originalBackgroundColor = new Color32(255, 235, 213, 255);
        selectedBackgroundColor = new Color32(255, 134, 0, 255);
        isClickDrag = false;
    }

    public virtual void Update()
    {
        if (isClickDrag)
        {
            // 滑鼠移動時，單點擊拖曳位置變換為滑鼠游標位置
            if (lastMousePos != Input.mousePosition)
            {
                lastMousePos = Input.mousePosition;
                skillSlotBeginDrag.icon.transform.position = lastMousePos;
            }
        }
    }

    public void OnSelect()
    {
        background.color = selectedBackgroundColor;
        SkillDescription.Instance.ResetSkillDescription(skill);

        // 當啟用單點擊拖曳時，若此Slot被選擇到，則移動拖曳的技能位置至此Slot旁
        if (isClickDrag)
        {
            skillSlotBeginDrag.icon.transform.position = this.transform.position + new Vector3(40f, 40f);
        }
    }

    public void OnDeselect()
    {
        background.color = originalBackgroundColor;
    }

    /// <summary>
    /// 單點擊拖曳功能 (滑鼠單點或是鍵盤點擊)
    /// </summary>
    public virtual void OnSlotClick()
    {
        if (isClickDrag && skillSlotBeginDrag != null)
        {
            ChangeSkillSlot(skillSlotBeginDrag);
            skillSlotBeginDrag.icon.transform.localPosition = Vector3.zero;
            skillSlotBeginDrag.slotCanvasSetting.overrideSorting = false;
            skillSlotBeginDrag.slotCanvasSetting.sortingOrder = originSortOrder;
            isClickDrag = false;
            return;
        }
        if (skill == null)
            return;

        skillSlotBeginDrag = this;
        skillSlotBeginDrag.slotCanvasSetting.overrideSorting = true;
        skillSlotBeginDrag.slotCanvasSetting.sortingOrder = 10;
        isClickDrag = true;
        lastMousePos = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        // 沒有技能則無法拖曳
        if (skill == null)
            return;

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
            this.GetComponent<Button>().Select();
        }
    }

    public virtual void OnPointerClick(PointerEventData e) 
    {
        if (skill == null)
            return;

        SkillDescription.Instance.ResetSkillDescription(skill);
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
            skillSlotBeginDrag.GetComponent<MenuSkillSlot>().linkSkillSlotOnCombatUI.AddSkill(sourceSlot.icon.sprite, sourceSlot.skill);
        }
    }
}

public enum SlotType
{
    Inventory,
    MenuHotKey
}