using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : MonoBehaviour
{
    public Vector2 pos;

    void Start()
    {
        string[] sp = name.Split('_');
        pos.x = int.Parse(sp[0]);
        pos.y = int.Parse(sp[1]);
    }

}
