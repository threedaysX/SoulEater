using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMove : MonoBehaviour, IPointerClickHandler
{
    public List<int> relativeX;
    public List<int> relativeY;
    public GameObject Canvas;
    GameObject startObj;
    GameObject targetObj;
    Vector3 startPos;
    Vector3 targetPos;
    public int triType;//0:Up 1:Down 
    public bool isDrag = false;

    void Start()
    {
        GetComponent<Image>().color = Color.cyan;
    }

    public void FixedUpdate()
    {
        //右鍵取消
        if (Input.GetKey(KeyCode.Mouse1)&& isDrag == true)
        {
            if (startObj != null) this.transform.position = startObj.transform.position;
            else this.transform.position = startPos;
            isDrag = false;
            GetComponent<Image>().color = Color.cyan;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        //移動
        if (isDrag == true)
        {
            Vector3 pos;
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            this.transform.position = pos;
        }

        //放下
        if (Input.GetKey(KeyCode.Mouse0) && isDrag == true)
        {
            targetObj = Canvas.GetComponent<RayCast>().Raycast();

            if (targetObj != null)
            {
                targetPos = targetObj.transform.position;
                int x = targetObj.GetComponent<Slot>().x;
                int y = targetObj.GetComponent<Slot>().y;
                bool emptySlot = true;
                //if list not occupy
                for (int i = 0; i < relativeX.Count; i++)
                {
                    if (targetObj.transform.parent.parent.parent.GetComponent<Endowment>().isOccupy[y + relativeY[i]][x + relativeX[i]]) emptySlot = false;
                }
                if (emptySlot && targetObj.GetComponent<Slot>().slotType == this.triType)
                {
                    Debug.Log("Yes");
                    //change bool value
                    if (startObj != null)
                    {
                        //if there's a last occupy. set to false?
                        for (int i = 0; i < relativeX.Count; i++)
                        {
                            Canvas.GetComponent<Endowment>().isOccupy[startObj.GetComponent<Slot>().y + relativeY[i]][startObj.GetComponent<Slot>().x + relativeX[i]] = false;
                            startObj.GetComponent<Image>().color = Color.white;
                        }
                    }
                    //occupy set to true
                    for (int i = 0; i < relativeX.Count; i++)
                    {
                        Canvas.GetComponent<Endowment>().isOccupy[y + relativeY[i]][x + relativeX[i]] = true;
                        targetObj.GetComponent<Image>().color = Color.red;
                    }
                    startObj = targetObj;

                    this.transform.position = targetPos;
                    isDrag = false;
                    GetComponent<Image>().color = Color.cyan;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }
                else
                {
                    Debug.Log("No");
                    if (startObj != null) this.transform.position = startObj.transform.position;
                    else this.transform.position = startPos;
                    isDrag = false;
                    GetComponent<Image>().color = Color.cyan;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }
            }
            //stay at current pos
            if (startObj != null)
            {
                //last occupy set to false?
                for (int i = 0; i < relativeX.Count; i++)
                {
                    Canvas.GetComponent<Endowment>().isOccupy[startObj.GetComponent<Slot>().y + relativeY[i]][startObj.GetComponent<Slot>().x + relativeX[i]] = false;
                    startObj.GetComponent<Image>().color = Color.white;
                }
            }
            isDrag = false;
            GetComponent<Image>().color = Color.cyan;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //拿起
        if (isDrag == false)
        {
            //first click
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPos.z = 0;
            GetComponent<Image>().color = Color.gray;
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            isDrag = true;
        }
    }
}
