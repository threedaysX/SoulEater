using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentData : Singleton<CurrentData>
{
    public int currentFragmentID;       //目前碎片
    public int currentStarID;           //存放當前點(三角形)在AllStar的ID<<中心點
    public Vector2 currentPos;           //存放當前點(三角形)在AllStar的ID<<中心點
    public int lastStarID;
    public List<int> coverStarsID = new List<int>();           //存放 此碎片會覆蓋到的點(三角形)們
    ///
    public GameObject followObj;
    public Vector2 tempOriginPos;
    ///
    public Text appearTriggerCount;
    public Image textImage;

    bool positionError = false;         //碎片位置卡到

    private void Start()
    {
        followObj = null;
        currentFragmentID = -1;
    }
    void Update()
    {
        if (followObj != null)
        {
            followObj.transform.position = Input.mousePosition;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
            return;
        if (ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.tag != "slot")
            return;

        if (currentFragmentID == -1)            //手中沒有碎片
        {

            if (Input.GetMouseButtonDown(1))//查看觸發多少條邊
            {
                if (AllFragment.Instance.fragments.Count == 0)
                    return;
                star getTemp = ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.GetComponent<star>();
                //Debug.Log(AllFragment.Instance.fragments[getTemp.fragID].m_Data.PrintTriggerCount());

                appearTriggerCount.text = AllFragment.Instance.fragments[getTemp.fragID].m_Data.PrintTriggerCount();
                ///
                appearTriggerCount.transform.position= Input.mousePosition;
                textImage.transform.position= Input.mousePosition;
                ///
                textImage.color = new Color(1, 1, 0, 0.7f);
                appearTriggerCount.color = new Color(0, 0, 0, 1);

            }
            if (Input.GetMouseButtonDown(0))    //拿起來
            {
                appearTriggerCount.text = "";
                textImage.color = new Color(0, 0, 0, 0);

                star getTemp = ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.GetComponent<star>();
                if (getTemp.isLocked)
                {
                    currentFragmentID = getTemp.fragID;
                    //Debug.Log("拿起來currentFragmentID:" + currentFragmentID);
                    //Debug.Log("拿起來fragmentID:" + getTemp.fragID);
                    Chip.Instance.PullUp(AllFragment.Instance.fragments[getTemp.fragID]);
                }
            }

        }
        else
        {                              //手中有碎片
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////確認好位置要放下了
            if (Input.GetMouseButtonDown(0) && !positionError)  //放下
            {
                if (followObj != null)
                {
                    followObj.transform.position = tempOriginPos;
                    followObj.GetComponent<Image>().raycastTarget = true;
                    followObj.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    followObj = null;
                    //Destroy(followObj);
                }

                Chip.Instance.PutOn(AllFragment.Instance.fragments[currentFragmentID], coverStarsID);

                //Debug.Log("放下currentFragmentID:" + currentFragmentID);
                //Debug.Log("放下ChipID:" + AllFragment.Instance.fragments[currentFragmentID].m_Data.fragmentID);
                coverStarsID.Clear();
                currentFragmentID = -1;
                lastStarID = -1;
                return;
            }

            currentStarID = ExtendedStandaloneInputModule.GetPointerEventData().pointerCurrentRaycast.gameObject.GetComponent<star>().allStar_ID;
            if (lastStarID == currentStarID)
                return;
            lastStarID = currentStarID;

            //先將舊的刷白
            for (int i = 0; i < coverStarsID.Count; i++)
            {
                AllStar.Instance.stars[coverStarsID[i]].ExitColor();
            }
            coverStarsID.Clear();

            List<Vector2> temp_FragStars;

            //計算新的CoverStars
            temp_FragStars = AllFragment.Instance.fragments[currentFragmentID].m_Data.touchPos_v2;
            currentPos = AllStar.Instance.stars[currentStarID].pos;

            if ((currentPos.x + currentPos.y) % 2 != 0)
            {
                currentPos.x -= 1;
            }

            positionError = false;
            for (int i = 0; i < temp_FragStars.Count; i++)
            {
                Vector2 tempVec = new Vector2(temp_FragStars[i].x + currentPos.x, temp_FragStars[i].y + currentPos.y);
                int id = AllStar.Instance.stars.FindIndex(x => x.pos == tempVec);
                if (id != -1)
                {
                    if (AllStar.Instance.stars[id].isLocked)
                    {
                        positionError = true;
                        continue;
                    }
                    coverStarsID.Add(id);
                }
            }
            //將新的上色
            if (positionError)
                for (int i = 0; i < coverStarsID.Count; i++)
                {
                    AllStar.Instance.stars[coverStarsID[i]].ErrorColor();
                }
            else
                for (int i = 0; i < coverStarsID.Count; i++)
                {
                    AllStar.Instance.stars[coverStarsID[i]].EnterColor();
                }

        }

    }

}
