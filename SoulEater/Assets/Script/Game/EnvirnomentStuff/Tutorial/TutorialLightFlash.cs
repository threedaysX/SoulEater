using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLightFlash : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.AttackKey1)) || 
            Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.AttackKey2)))
        {

        }

    }
}
