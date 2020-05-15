using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrentData : MonoBehaviour
{
    public Fragment currentFragment;    //目前碎片圖案會用到那些相對位置
    public int currentStarID;           //存放當前點(三角形)在AllStar的ID<<中心點
    public int lastStarID;
    public List<star> CoverStars = new List<star>();           //存放 此碎片會覆蓋到的點(三角形)們
    List<Vector2> temp_touchStars;
    Vector2 currentVec;

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            return;
        if (ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.tag!="slot")
            return;
        //Debug.Log("Clicked on the UI");
        //Debug.Log(ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.name);
        currentStarID = ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.GetComponent<star>().allStar_ID;
        if (lastStarID == currentStarID)
            return;
        lastStarID = currentStarID;

        //先將舊的刷白
        for (int i = 0; i < CoverStars.Count; i++)
        {
            CoverStars[i].ExitColor();
        }
        CoverStars.Clear();
        
        //計算新的CoverStars
        temp_touchStars = currentFragment.m_Data.touchStars;
        currentVec = AllStar.Instance.stars[currentStarID].pos;

        if ((currentVec.x+ currentVec.y) % 2 != 0)
            return;
        
        for (int i = 0; i < temp_touchStars.Count; i++)
        {
            Vector2 tempVec = new Vector2(temp_touchStars[i].x + currentVec.x, temp_touchStars[i].y + currentVec.y);
            int id = AllStar.Instance.stars.FindIndex(x => x.pos == tempVec);
            if (id != -1)
            {
                CoverStars.Add(AllStar.Instance.stars[id]);
            }
        }
        //將新的上色
        for (int i = 0; i < CoverStars.Count; i++)
        {
            CoverStars[i].EnterColor();
        }



    }

}
