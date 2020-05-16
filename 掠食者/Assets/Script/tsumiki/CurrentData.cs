﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrentData : MonoBehaviour
{
    public Fragment currentFragment;    //目前碎片圖案會用到那些相對位置
    public int currentStarID;           //存放當前點(三角形)在AllStar的ID<<中心點
    public int lastStarID;
    public List<star> CoverStars = new List<star>();           //存放 此碎片會覆蓋到的點(三角形)們
    List<Vector2> temp_FragStars;
    Vector2 currentVec;
    bool error = false;
    bool putOn = false;

    void Update()
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////確認好位置要放下了
        if (Input.GetMouseButtonDown(0) && !error)
        {
            putOn = true;
            chip tempChip = new chip();
            tempChip.touchStars = CoverStars;
            tempChip.PutOn();

            AllChips.Instance.chips.Add(tempChip);

        }
        if (!EventSystem.current.IsPointerOverGameObject())
            return;
        if (ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.tag != "slot")
            return;
        //Debug.Log("Clicked on the UI");
        //Debug.Log(ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.name);
        currentStarID = ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.GetComponent<star>().allStar_ID;
        if (lastStarID == currentStarID)
            return;
        lastStarID = currentStarID;

        //先將舊的刷白
        if (!putOn)
        {
            for (int i = 0; i < CoverStars.Count; i++)
            {
                CoverStars[i].ExitColor();
            }
        }
        CoverStars.Clear();
        putOn = false;

        //計算新的CoverStars
        temp_FragStars = currentFragment.m_Data.touchStars;
        currentVec = AllStar.Instance.stars[currentStarID].pos;

        if ((currentVec.x + currentVec.y) % 2 != 0)
            return;

        error = false;
        for (int i = 0; i < temp_FragStars.Count; i++)
        {
            Vector2 tempVec = new Vector2(temp_FragStars[i].x + currentVec.x, temp_FragStars[i].y + currentVec.y);
            int id = AllStar.Instance.stars.FindIndex(x => x.pos == tempVec);
            if (id != -1)
            {
                if (AllStar.Instance.stars[id].isLocked)
                {
                    error = true;
                    continue;
                }
                CoverStars.Add(AllStar.Instance.stars[id]);
            }
        }
        //將新的上色
        if (error)
            for (int i = 0; i < CoverStars.Count; i++)
            {
                CoverStars[i].ErrorColor();
            }
        else
            for (int i = 0; i < CoverStars.Count; i++)
            {
                CoverStars[i].EnterColor();
            }



    }

}
