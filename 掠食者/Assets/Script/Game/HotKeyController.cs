using System.Collections.Generic;
using UnityEngine;

public class HotKeyController : Singleton<HotKeyController>
{
    public const KeyCode moveRight = KeyCode.RightArrow;
    public const KeyCode moveLeft = KeyCode.LeftArrow;
    public const KeyCode moveUp = KeyCode.UpArrow;
    public const KeyCode moveDown = KeyCode.DownArrow;

    public const KeyCode attackKey1 = KeyCode.Z;
    public const KeyCode attackKey2 = KeyCode.X;
    public const KeyCode jumpKey = KeyCode.Space;
    public const KeyCode collectKey = KeyCode.C;
    public const KeyCode evadeKey = KeyCode.LeftShift;
    public const KeyCode skillKey1 = KeyCode.A;
    public const KeyCode skillKey2 = KeyCode.S;
    public const KeyCode skillKey3 = KeyCode.D;
    public const KeyCode skillKey4 = KeyCode.F;
    public const KeyCode skillKey5 = KeyCode.Q;
    public const KeyCode skillKey6 = KeyCode.W;
    public const KeyCode skillKey7 = KeyCode.E;
    public const KeyCode skillKey8 = KeyCode.R;

    public const KeyCode menuKey = KeyCode.Escape;
    public const KeyCode openMapKey = KeyCode.Tab;

    public Dictionary<string, KeyCode> keyCodeDict;

    public void Start()
    {
        
    }

    public void ModifyKey()
    {

    }
}
