using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//x是偶數  >>下三角   >>左右上
//x是奇數  >>上三角   >>左右下

//所有碎片繼承此物件
public class chip : MonoBehaviour
{
    int triggerCount;

    public List<Vector2> touchStars = new List<Vector2>();          //用來記錄此碎片和哪個star交疊

    public List<star> neighborStars = new List<star>();             //此碎片邊上的鄰居star

    public List<List<Vector2>> neighborRelative = new List<List<Vector2>>();//此碎片邊上的鄰居關係

    /*有待替換，此方法有bug*/
    void OnTriggerEnter2D(Collider2D col)                           //當此碎片進入star重疊時
    {
        //將此star的位置存到touchStars
        touchStars.Add(col.GetComponent<star>().pos);

        FreshNeighborStars();   //只有在新增加(進入)star時，才需重新計算(因為 離開star時 也只需用到舊有的鄰居資訊)
        LockStar(col);
        ThisReDetectEdge();
        NeighborReDetectEdge();
    }
    /*有待替換，此方法有bug*/
    void OnTriggerExit2D(Collider2D col)                            //當此碎片離開star重疊時
    {
        //將此star拿出touchStars
        int id = touchStars.FindIndex(x => x == col.GetComponent<star>().pos);
        if (id != -1)
            touchStars.RemoveAt(id);

        UnlockStar(col);
        ThisReDetectEdge();
        NeighborReDetectEdge();
    }

    public void LockStar(Collider2D col)
    {
        //star變紅色
        col.GetComponent<SpriteRenderer>().color = Color.red;
        //更改Star狀態
        col.GetComponent<star>().isLocked = true;
        //Star綁定chip_script
        col.GetComponent<star>().chip_script = this;
    }

    public void UnlockStar(Collider2D col)
    {
        //star變回白色
        col.GetComponent<SpriteRenderer>().color = Color.white;
        //更改Star狀態
        col.GetComponent<star>().isLocked = false;
        //Star取消綁定chip_script
        col.GetComponent<star>().chip_script = null;
    }

    //此碎片重新計算邊的觸發
    public void ThisReDetectEdge()//////////////////////////////////////////////////////////////////////////////////
    {
        //中多少條邊就triggerCount++

        //先觀察有哪些鄰居存在

    }

    //用鄰居觀察邊的關係，以***相對位置***紀錄
    public void Relative()//////////////////////////////////////////////////////////////////////////////////
    {
        ///ex 
        ///相差(2,0),(1,2),(-1,2)必為同一邊
        
        List<Vector2> temp = new List<Vector2>();
        temp.Add(new Vector2(2, 0));

        neighborRelative.Add(temp);
    }


    //鄰居碎片們重新計算邊的觸發
    public void NeighborReDetectEdge()
    {
        //呼叫鄰居的ThisReDetectEdge
        for (int i = 0; i < neighborStars.Count; i++)
            neighborStars[i].chip_script.ThisReDetectEdge();
    }

    public void FreshNeighborStars()
    {
        neighborStars.Clear();

        for (int i = 0; i < touchStars.Count; i++)
        {
            //左邊的star pos
            NeighborFunc(new Vector2(touchStars[i].x - 1, touchStars[i].y));
            //右邊的star pos
            NeighborFunc(new Vector2(touchStars[i].x + 1, touchStars[i].y));

            if (touchStars[i].x % 2 == 0)   //x是偶數，鄰居在左右上
                NeighborFunc(new Vector2(touchStars[i].x, touchStars[i].y + 2));     //上邊的star pos
            if (touchStars[i].x % 2 == 0)   //x是奇數，鄰居在左右下
                NeighborFunc(new Vector2(touchStars[i].x, touchStars[i].y - 2));     //下邊的star pos

        }
    }

    private void NeighborFunc(Vector2 Star_pos)
    {
        int id = touchStars.FindIndex(x => x == Star_pos);  //檢查是否屬於此碎片的一部分
        if (id != -1)       //此碎片有此點
            return;       //跳過

        //找到該點
        id = AllStar.Instance.stars.FindIndex(x => x.pos == Star_pos);
        if (id != -1)
        {
            //放入neighborStars
            neighborStars.Add(AllStar.Instance.stars[id]);
        }
    }


}
