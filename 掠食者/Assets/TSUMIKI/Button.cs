using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public void Push()
    {
        for(int i=0;i< AllChips.Instance.chips.Count; i++)
        {
            Debug.Log("*** " + AllChips.Instance.chips[i].gameObject.name + " * triggerCount ***" + AllChips.Instance.chips[i].triggerCount);
            
        }
    }
}
