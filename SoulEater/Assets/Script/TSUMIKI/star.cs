using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class star : MonoBehaviour
{
    public int fragID;
    public Vector2 pos;
    public bool isLocked;
    public int allStar_ID;

    void Awake()
    {
        fragID = -1;
        string[] sp = name.Split('_');
        pos.x = int.Parse(sp[0]);
        pos.y = int.Parse(sp[1]);
        isLocked = false;
        //chip_script = null;
        allStar_ID = AllStar.Instance.stars.Count;
        AllStar.Instance.stars.Add(this);
    }
    public void EnterColor()
    {
        this.gameObject.GetComponent<Image>().color = Color.blue;
    }
    public void ErrorColor()
    {
        this.gameObject.GetComponent<Image>().color = Color.red;
    }
    public void ExitColor()
    {
        this.gameObject.GetComponent<Image>().color = Color.white;
    }
}

