using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSlotBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Button Related")]
    public SlotType slotType;
    public Image background;
    public Image icon;

    protected static ButtonSlotBase slotBeginDrag;
    protected static bool inDrag;
    protected static bool isClickDrag;
    protected static bool mouseOnSlotChecked = false;
    protected static bool inMouseMoving;
    protected static Vector3 lastMousePos;

    protected int originSortOrder;
    protected Canvas slotCanvasSetting;

    private void Awake()
    {
        background = GetComponent<Image>();
        slotCanvasSetting = icon.GetComponent<Canvas>();
    }

    protected virtual void Start()
    {
        originSortOrder = slotCanvasSetting.sortingOrder;
        isClickDrag = false;
        GetComponent<Button>().onClick.AddListener(OnSlotClick);
    }

    protected virtual void Update()
    {
        // Mouse Click To Drag.
        if (isClickDrag)
        {
            if (!inDrag)
            {
                inDrag = true;
            }

            // 滑鼠移動時，單點擊拖曳位置變換為滑鼠游標位置
            if (lastMousePos != GetMousePositionOnScreen())
            {
                lastMousePos = GetMousePositionOnScreen();
                slotBeginDrag.icon.transform.position = lastMousePos;
                inMouseMoving = true;
            }
            else
            {
                if (inMouseMoving)
                    inMouseMoving = false;
            }

            // Cancel Click Drag.
            if ((Input.GetKeyDown(KeyCode.Mouse0) && !mouseOnSlotChecked) || Input.GetKeyDown(KeyCode.Mouse1)
                || Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.EscMenuKey)))
            {
                ResetImagePosition(slotBeginDrag.icon);
                isClickDrag = false;
                inDrag = false;
            }
        }
    }

    public static bool GetDragingState()
    {
        return inDrag;
    }

    protected Vector3 GetMousePositionOnScreen()
    {
        var pos = Input.mousePosition;
        pos.z = 10;
        return Camera.main.ScreenToWorldPoint(pos);
    }

    public virtual void OnSelect(BaseEventData e)
    {
        // 當啟用單點擊拖曳時，若此Slot被選擇到，則移動拖曳的技能位置至此Slot旁
        if (isClickDrag && !inMouseMoving)
        {
            slotBeginDrag.icon.transform.position = this.transform.position + new Vector3(0.2f, 0.2f);
        }
    }

    public virtual void OnDeselect(BaseEventData e)
    {
        
    }

    public virtual void OnBeginDrag(PointerEventData e)
    {
        // 只能用滑鼠左鍵拖曳
        if (!Input.GetMouseButton(0))
            return;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        slotBeginDrag = this;
        inDrag = true;

        slotCanvasSetting.overrideSorting = true;
        slotCanvasSetting.sortingOrder = 99;
    }

    public virtual void OnDrag(PointerEventData e)
    {
        // 只能用滑鼠左鍵拖曳
        if (!Input.GetMouseButton(0))
            return;
        icon.transform.position = GetMousePositionOnScreen();
    }

    public virtual void OnEndDrag(PointerEventData e)
    {
        inDrag = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        ResetImagePosition(icon);
        slotCanvasSetting.sortingOrder = originSortOrder;
    }

    public virtual void OnDrop(PointerEventData e)
    {
        this.GetComponent<Button>().Select();
    }

    /// <summary>
    /// 單點擊拖曳功能 (滑鼠單點或是鍵盤點擊)
    /// </summary>
    public virtual void OnSlotClick()
    {
        // If in draging, put down that item and return, avoid pick up again.
        if (OnSlotClickPutDown())
            return;

        OnSlotClickPickUp();
    }

    protected virtual bool OnSlotClickPutDown()
    {
        if (isClickDrag && slotBeginDrag != null)
        {
            ChangeSlot(slotBeginDrag);
            ResetImagePosition(slotBeginDrag.icon);
            slotBeginDrag.slotCanvasSetting.sortingOrder = originSortOrder;
            isClickDrag = false;
            inDrag = false;
            return true;
        }
        return false;
    }

    protected virtual void OnSlotClickPickUp()
    {
        slotBeginDrag = this;
        inDrag = true;
        slotBeginDrag.slotCanvasSetting.overrideSorting = true;
        slotBeginDrag.slotCanvasSetting.sortingOrder = 99;
        isClickDrag = true;
        lastMousePos = GetMousePositionOnScreen();
    }

    public virtual void OnPointerClick(PointerEventData e)
    {
        
    }

    /// <summary>
    /// Check if player pointer on slot(s), then reset checked state.
    /// </summary>
    public void OnPointerEnter(PointerEventData e)
    {
        mouseOnSlotChecked = true;
    }

    public void OnPointerExit(PointerEventData e)
    {
        mouseOnSlotChecked = false;
    }

    public virtual void ChangeSlot(ButtonSlotBase sourceSlot)
    {
        Sprite sourceIcon = sourceSlot.icon.sprite;
        sourceSlot.AddSlot(this.icon.sprite);
        AddSlot(sourceIcon);
    }

    public virtual void AddSlot(Sprite icon)
    {
        this.icon.sprite = icon;
    }

    public virtual void RemoveSlot()
    {
        this.icon.sprite = null;
    }

    protected void ResetImagePosition(Image image)
    {
        image.transform.localPosition = Vector3.zero;
        image.transform.GetComponent<RectTransform>().position = Vector3.zero;
        image.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
}

public enum SlotType
{
    KeyBoard,
    Inventory,
    MenuHotKey
}
