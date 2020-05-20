using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrentData : MonoBehaviour
{
    public Fragment testFrag;
    public Fragment testFrag2;
    public int currentFragmentID;       //目前碎片
    public int testFragmentID;
    public int currentStarID;           //存放當前點(三角形)在AllStar的ID<<中心點
    public int lastStarID;
    public List<int> CoverStarsID = new List<int>();           //存放 此碎片會覆蓋到的點(三角形)們

    bool positionError = false;         //碎片位置卡到

    private void Start()
    {
        AllFragment.Instance.fragments.Add(new Fragment());
        currentFragmentID = AllFragment.Instance.fragments.Count - 1;
        AllFragment.Instance.fragments[currentFragmentID].m_Data = new F_Data();
        AllFragment.Instance.fragments[currentFragmentID].m_Data.touchStars_v2 = testFrag.m_Data.touchStars_v2;
        AllFragment.Instance.fragments[currentFragmentID].m_Data.fragmentID = currentFragmentID;

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            testFrag = testFrag2;
            AllFragment.Instance.fragments.Add(new Fragment());
            currentFragmentID = AllFragment.Instance.fragments.Count - 1;
            AllFragment.Instance.fragments[currentFragmentID].m_Data = new F_Data();
            AllFragment.Instance.fragments[currentFragmentID].m_Data.touchStars_v2 = testFrag2.m_Data.touchStars_v2;
            AllFragment.Instance.fragments[currentFragmentID].m_Data.fragmentID = currentFragmentID;

        }
        if (!EventSystem.current.IsPointerOverGameObject())
            return;
        if (ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.tag != "slot")
            return;

        if (currentFragmentID == -1)            //手中沒有碎片
        {
            if (Input.GetMouseButtonDown(0))    //拿起來
            {
                star getTemp = ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.GetComponent<star>();
                if (getTemp.isLocked)
                {
                    currentFragmentID = getTemp.fragID;
                    Debug.Log("拿起來currentFragmentID:" + currentFragmentID);
                    Debug.Log("拿起來fragmentID:" + getTemp.fragID);
                    Chip.Instance.PullUp(AllFragment.Instance.fragments[getTemp.fragID]);
                }
            }

        }
        else
        {                              //手中有碎片
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////確認好位置要放下了
            if (Input.GetMouseButtonDown(0) && !positionError)  //放下
            {
                AllFragment.Instance.fragments[currentFragmentID].m_Data.touchStarsID.Clear();
                
                for (int i = 0; i < CoverStarsID.Count; i++)
                    AllFragment.Instance.fragments[currentFragmentID].m_Data.touchStarsID.Add( CoverStarsID[i]);

                Chip.Instance.PutOn(AllFragment.Instance.fragments[currentFragmentID]);

                Debug.Log("放下currentFragmentID:" + currentFragmentID);
                Debug.Log("放下ChipID:" + AllFragment.Instance.fragments[currentFragmentID].m_Data.fragmentID);
                CoverStarsID.Clear();
                currentFragmentID = -1;
                lastStarID = -1;
                return;
            }

            currentStarID = ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.GetComponent<star>().allStar_ID;
            if (lastStarID == currentStarID)
                return;
            lastStarID = currentStarID;

            //先將舊的刷白
            for (int i = 0; i < CoverStarsID.Count; i++)
            {
                AllStar.Instance.stars[CoverStarsID[i]].ExitColor();
            }
            CoverStarsID.Clear();

            List<Vector2> temp_FragStars;
            Vector2 currentVec;

            //計算新的CoverStars
            temp_FragStars = AllFragment.Instance.fragments[currentFragmentID].m_Data.touchStars_v2;
            currentVec = AllStar.Instance.stars[currentStarID].pos;

            if ((currentVec.x + currentVec.y) % 2 != 0)
            {
                currentVec.x -= 1;
            }

            positionError = false;
            for (int i = 0; i < temp_FragStars.Count; i++)
            {
                Vector2 tempVec = new Vector2(temp_FragStars[i].x + currentVec.x, temp_FragStars[i].y + currentVec.y);
                int id = AllStar.Instance.stars.FindIndex(x => x.pos == tempVec);
                if (id != -1)
                {
                    if (AllStar.Instance.stars[id].isLocked)
                    {
                        positionError = true;
                        continue;
                    }
                    CoverStarsID.Add(id);
                }
            }
            //將新的上色
            if (positionError)
                for (int i = 0; i < CoverStarsID.Count; i++)
                {
                    AllStar.Instance.stars[CoverStarsID[i]].ErrorColor();
                }
            else
                for (int i = 0; i < CoverStarsID.Count; i++)
                {
                    AllStar.Instance.stars[CoverStarsID[i]].EnterColor();
                }

        }

    }

}
