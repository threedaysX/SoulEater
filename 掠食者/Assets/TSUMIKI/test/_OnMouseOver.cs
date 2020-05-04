using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _OnMouseOver : MonoBehaviour
{

    void OnMouseOver()
    {
        Debug.Log(this.name+GetComponent<fragment>().pos);
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = Color.white;

    }
}
