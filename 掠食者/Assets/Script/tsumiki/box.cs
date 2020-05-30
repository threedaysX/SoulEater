using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Box : MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler
{
    public Fragment PutFrag;
    public string fName;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(BtnDown);
    }

    public void BtnDown()
    {
        AllFragment.Instance.fragments.Add(new Fragment());
        int id = AllFragment.Instance.fragments.Count - 1;
        AllFragment.Instance.fragments[id].m_Data = new F_Data();
        CurrentData.Instance.currentFragmentID = id;
        AllFragment.Instance.fragments[id].m_Data.init(fName,PutFrag.m_Data.touchPos_v2,id);
    }

    public void OnDrag(PointerEventData e)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData e)
    {
        
    }

    public void OnDrop(PointerEventData e)
    {

    }
}