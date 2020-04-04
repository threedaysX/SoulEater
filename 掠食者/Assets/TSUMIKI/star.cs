using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : MonoBehaviour
{
    public chip chip_script;
    public Vector2 pos;
    public bool isLocked;

    void Awake()
    {
        string[] sp = name.Split('_');
        pos.x = int.Parse(sp[0]);
        pos.y = int.Parse(sp[1]);
        isLocked = false;
        chip_script = null;

        AllStar.Instance.stars.Add(this);
    }
}

