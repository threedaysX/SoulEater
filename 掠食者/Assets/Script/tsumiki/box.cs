using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Box : MonoBehaviour, IPointerDownHandler
{
    public Fragment PutFrag;
    public string fName;
    public int fId;

    private void Start()
    {
        fId = -1;
        //this.GetComponent<Button>().onClick.AddListener(BtnDown);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AllFragment.Instance.fragments.Add(ScriptableObject.CreateInstance("Fragment") as Fragment);
        int id = AllFragment.Instance.fragments.Count - 1;
        fId = id;
        AllFragment.Instance.fragments[id].m_Data = new F_Data();
        CurrentData.Instance.currentFragmentID = id;
        AllFragment.Instance.fragments[id].m_Data.init(fName, PutFrag.m_Data.touchPos_v2, id);

        this.GetComponent<Image>().color = new Color(1, 1, 1, 0.6f);
        CurrentData.Instance.followObj = this.gameObject;
        CurrentData.Instance.followObj.AddComponent<PolygonCollider2D>();
        CurrentData.Instance.followObj.GetComponent<Image>().raycastTarget = false;
    }
}