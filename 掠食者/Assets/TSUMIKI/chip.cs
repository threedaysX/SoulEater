using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//x是偶數  >>下三角   >>左右上
//x是奇數  >>上三角   >>左右下

public class chip : MonoBehaviour
{
    public List<Vector2> stars = new List<Vector2>();       //目前這個chip會和哪些點重和

    void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<SpriteRenderer>().color = Color.red;

        stars.Add(col.GetComponent<star>().pos);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        col.GetComponent<SpriteRenderer>().color = Color.white;

        int id = stars.FindIndex(x => x == col.GetComponent<star>().pos);
        if (id != -1)
            stars.RemoveAt(id);

    }

    
}
