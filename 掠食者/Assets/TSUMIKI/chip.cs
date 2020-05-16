using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 碎片限制
 1.三角形擺放不得超過三層
 2.碎片必連貫且不得中空
*/
/*
 程式碼講解

 使用方法:
 這個檔案chip.cs是給任何形狀的碎片使用
 只要將那個碎片物件裝上polygon collider 2D和Rigidbody 2D和此chip即可使用
 
 原理:
 碎片擺定位置後，根據佔用哪些星星點，即可知道碎片的形狀
 目前的座標表示方式如圖(星星座標.jpg)
 可知 當   x+y是偶數  即為  下三角  產生的邊有  左右上
                奇數        上三角              左右下
 
 當碎片更改位置或形狀(任何會更改使用星星的狀況)後，需手動呼叫newStart()重新計算
 
    
 執行 後，會自動往下依序執行
 1. FreshNeighborStars()    >>  根據占用的星星推估此碎片有"哪些鄰居星星"和"觸發邊位置(鄰居星星的角度)"，
                                最後得到對於這個碎片哪些星星構成一條邊，存入neighborRelative
 2. ThisReDetectEdge()      >>  判斷哪些邊有完全觸發，triggerCount++
 3. NeighborReDetectEdge()  >>  提醒鄰居也要重新計算觸發邊
 */

//所有碎片繼承此物件
public class chip : MonoBehaviour
{
    public int triggerCount;
    [SerializeField]
    public List<Vector2> touchStars = new List<Vector2>();                  //用來記錄此碎片和哪個star交疊
    [SerializeField]
    public List<Neighbor> neighborStars = new List<Neighbor>();             //此碎片邊上的鄰居star
    [SerializeField]
    public List<neighborList> neighborRelative = new List<neighborList>();  //此碎片邊上的鄰居關係

    void Awake()
    {
        AllChips.Instance.chips.Add(this);
    }

    void NewStart()
    {
        touchStars.Clear(); //清空
        //重新計算有哪些stars

    }
    /*有待替換，此方法有bug*/
    void OnTriggerEnter2D(Collider2D col)                           //當此碎片進入star重疊時
    {
        //將此star的位置存到touchStars
        touchStars.Add(col.GetComponent<star>().pos);

        LockStar(col);
        FreshNeighborStars();   //只有在新增加(進入)star時，才需重新計算(因為 離開star時 也只需用到舊有的鄰居資訊)
        ThisReDetectEdge();
        NeighborReDetectEdge();
    }
    /*有待替換，此方法有bug*/
    void OnTriggerExit2D(Collider2D col)                            //當此碎片離開star重疊時
    {
        //將此star拿出touchStars    不改好像也沒關係
        int id = touchStars.FindIndex(x => x == col.GetComponent<star>().pos);
        if (id != -1)
            touchStars.RemoveAt(id);

        UnlockStar(col);
        NeighborReDetectEdge();
        FreshNeighborStars();
        ThisReDetectEdge();

    }

    public void LockStar(Collider2D col)////////////////////////////////////////////////////////////OK
    {
        if (col.GetComponent<star>().chip_script != null)
            return;
        //star變紅色
        col.GetComponent<SpriteRenderer>().color = Color.red;
        //更改Star狀態
        col.GetComponent<star>().isLocked = true;
        //Star綁定chip_script
        col.GetComponent<star>().chip_script = this;
    }

    public void UnlockStar(Collider2D col)//////////////////////////////////////////////////////////OK
    {
        if (col.GetComponent<star>().chip_script.gameObject.name != this.name)
            return;
        //star變回白色
        col.GetComponent<SpriteRenderer>().color = Color.white;
        //更改Star狀態
        col.GetComponent<star>().isLocked = false;
        //Star取消綁定chip_script
        col.GetComponent<star>().chip_script = null;
    }

    //重新計算此碎片的觸發邊
    public void ThisReDetectEdge()/////////////////////////////////////////////////////////////////OK
    {
        triggerCount = 0;               //中多少條邊就triggerCount++

        //先觀察有哪些鄰居存在
        for (int i = 0; i < neighborRelative.Count; i++)
        {
            bool allOk = true;
            for (int j = 0; j < neighborRelative[i].listNei.Count; j++)
            {
                if (neighborRelative[i].listNei[j]._Star.chip_script == null)
                {
                    allOk = false;
                    break;
                }
            }
            if (allOk)
            {
                triggerCount++;
            }
        }
    }

    //用鄰居觀察邊的關係，完成 neighborRelative
    public void NeighborRelative()////////////////////////////////////////////////////////////////OK
    {
        List<Neighbor> nei_D_U = new List<Neighbor>();//存放 下三角形的鄰居中，以 上邊 觸發的三角形
        List<Neighbor> nei_D_R = new List<Neighbor>();//                          右邊
        List<Neighbor> nei_D_L = new List<Neighbor>();//                          左邊
        List<Neighbor> nei_U_L = new List<Neighbor>();//     上三角               左邊
        List<Neighbor> nei_U_R = new List<Neighbor>();//                          右邊
        List<Neighbor> nei_U_D = new List<Neighbor>();//                          下邊

        for (int i = 0; i < neighborStars.Count; i++)
        {
            if (neighborStars[i].tri == 0)
            {
                switch (neighborStars[i].toggleEdge)
                {
                    case 1:
                        nei_D_U.Add(neighborStars[i]);
                        break;
                    case 2:
                        nei_D_R.Add(neighborStars[i]);
                        break;
                    case 3:
                        nei_D_L.Add(neighborStars[i]);
                        break;
                }
            }
            else
            {
                switch (neighborStars[i].toggleEdge)
                {
                    case 3:
                        nei_U_L.Add(neighborStars[i]);
                        break;
                    case 2:
                        nei_U_R.Add(neighborStars[i]);
                        break;
                    case 0:
                        nei_U_D.Add(neighborStars[i]);
                        break;
                }
            }
        }
        bubbleSort(nei_D_U);    //排序 nei_D_U >>以x座標大小排序，由小到大
        bubbleSort(nei_U_D);
        bubbleSort(nei_D_R);
        bubbleSort(nei_D_L);
        bubbleSort(nei_U_L);
        bubbleSort(nei_U_R);
        neighborRelative.Clear();
        CutEdge(nei_D_U, "nei_D_U");    //只要中間斷掉 即得到一個邊
        CutEdge(nei_U_D, "nei_U_D");
        CutEdge(nei_D_R, "nei_D_R");
        CutEdge(nei_D_L, "nei_D_L");
        CutEdge(nei_U_L, "nei_U_L");
        CutEdge(nei_U_R, "nei_U_R");

    }

    void CutEdge(List<Neighbor> list, string debug_name)//////////////////////////////////////////////OK
    {
        if (list.Count == 0) return;
        neighborList temp = new neighborList();
        temp.listNei.Add(list[0]);

        if (list.Count == 1)
        {
            neighborRelative.Add(temp);
            return;
        }
        Neighbor lastNei = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            bool juge = false;
            //nei_D_U   >>判斷連續方式  >>x+2
            //nei_U_D   >>判斷連續方式  >>x+2
            //nei_D_R   >>判斷連續方式  >>x+1，y+1
            //nei_D_L   >>判斷連續方式  >>x+1，y-1
            //nei_U_L   >>判斷連續方式  >>x+1，y+1
            //nei_U_R   >>判斷連續方式  >>x+1，y-1

            switch (debug_name)
            {
                case "nei_D_U":
                    juge = (list[i]._Star.pos.x == lastNei._Star.pos.x + 2);
                    break;
                case "nei_U_D":
                    juge = (list[i]._Star.pos.x == lastNei._Star.pos.x + 2);
                    break;
                case "nei_D_R":
                    juge = (list[i]._Star.pos.x == lastNei._Star.pos.x + 1) && (list[i]._Star.pos.y == lastNei._Star.pos.y + 1);
                    break;
                case "nei_U_L":
                    juge = (list[i]._Star.pos.x == lastNei._Star.pos.x + 1) && (list[i]._Star.pos.y == lastNei._Star.pos.y + 1);
                    break;
                case "nei_D_L":
                    juge = (list[i]._Star.pos.x == lastNei._Star.pos.x + 1) && (list[i]._Star.pos.y == lastNei._Star.pos.y - 1);
                    break;
                case "nei_U_R":
                    juge = (list[i]._Star.pos.x == lastNei._Star.pos.x + 1) && (list[i]._Star.pos.y == lastNei._Star.pos.y - 1);
                    break;
                default:
                    Debug.LogError("error");
                    break;
            }
            if (juge)
            {
                temp.listNei.Add(list[i]);
                lastNei = list[i];

                if (i == list.Count - 1)
                    neighborRelative.Add(temp);
            }else
            {
                neighborRelative.Add(temp);
                temp.listNei.Clear();
                temp.listNei.Add(list[i]);
                lastNei = list[i];
            }
        }
    }

    void bubbleSort(List<Neighbor> list)///////////////////////////////////////////////////////////OK
    {
        if (list.Count <= 1) return;
        for (int i = 0; i < list.Count; i++)
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[i]._Star.pos.x > list[j]._Star.pos.x)
                {
                    //swap
                    Neighbor temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
    }

    //鄰居碎片們重新計算邊的觸發
    public void NeighborReDetectEdge()//////////////////////////////////////////////////////////OK
    {
        if (neighborStars.Count == 0) return;
        //呼叫鄰居的ThisReDetectEdge
        for (int i = 0; i < neighborStars.Count; i++)
        {
            if (neighborStars[i]._Star.chip_script != null)
                neighborStars[i]._Star.chip_script.ThisReDetectEdge();
        }
    }

    public void FreshNeighborStars()/////////////////////////////////////////////////////////// OK
    {
        neighborStars.Clear();
        for (int i = 0; i < touchStars.Count; i++)
        {
            if ((touchStars[i].x + touchStars[i].y) % 2 == 0)       //x+y是偶數 >> touchStars是 倒三角 >> 鄰居在左右上
            {
                NeighborFunc(new Vector2(touchStars[i].x - 1, touchStars[i].y), 1, 2);  //左邊的star pos 是正三角 觸發的邊是正三角的右邊
                NeighborFunc(new Vector2(touchStars[i].x + 1, touchStars[i].y), 1, 3);  //右邊的star pos 是正三角 觸發的邊是正三角的左邊
                NeighborFunc(new Vector2(touchStars[i].x, touchStars[i].y + 1), 1, 0);  //上邊的star pos 是正三角 觸發的邊是正三角的下邊  
            }
            else                                                   //x+y是奇數 >> touchStars是 正三角 >> 鄰居在左右下
            {
                NeighborFunc(new Vector2(touchStars[i].x - 1, touchStars[i].y), 0, 2);  //左邊的star pos 是倒三角 觸發的邊是倒三角的右邊
                NeighborFunc(new Vector2(touchStars[i].x + 1, touchStars[i].y), 0, 3);  //右邊的star pos 是倒三角 觸發的邊是倒三角的左邊
                NeighborFunc(new Vector2(touchStars[i].x, touchStars[i].y - 1), 0, 1);  //下邊的star pos 是倒三角 觸發的邊是倒三角的上邊
            }
        }
        NeighborRelative();             //之後可考慮用相對位置，這樣只有在旋轉的時候才需要重新計算
    }

    private void NeighborFunc(Vector2 Star_pos, int _tri, int _to)///////////////////////////////// OK
    {
        int id = touchStars.FindIndex(x => x == Star_pos);  //檢查是否屬於此碎片的一部分
        if (id != -1)       //此碎片有此點
            return;       //跳過

        //找到該點
        id = AllStar.Instance.stars.FindIndex(x => x.pos == Star_pos);
        if (id != -1)
        {
            Neighbor temp = new Neighbor(AllStar.Instance.stars[id], _tri, _to);
            //放入neighborStars
            neighborStars.Add(temp);
        }
    }


}

[System.Serializable]
public class Neighbor//////////////////////////////////////////////////////////////////////////// OK
{
    public star _Star;
    public int tri;            //UpTriangle=1  //DownTriangle=0
    public int toggleEdge;     //Up    1       //Down   0          //Left   3          //Right  2
    public Neighbor(star _S, int _tri, int _to)
    {
        _Star = _S;
        tri = _tri;
        toggleEdge = _to;
    }
}
[System.Serializable]
public class neighborList//////////////////////////////////////////////////////////////////////////// OK
{
    public List<Neighbor> listNei = new List<Neighbor>();
}
