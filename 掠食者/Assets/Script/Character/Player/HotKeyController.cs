using UnityEngine;

public class HotKeyController : Singleton<HotKeyController>
{
    public static KeyCode moveRight;
    public static KeyCode moveLeft;
    public static KeyCode moveUp;
    public static KeyCode moveDown;

    public static KeyCode attackKey;
    public static KeyCode jumpKey;
    public static KeyCode collectKey;
    public static KeyCode evadeKey;
    public static KeyCode skillKey1;
    public static KeyCode skillKey2;
    public static KeyCode skillKey3;
    public static KeyCode skillKey4;
    public static KeyCode skillKey5;
    public static KeyCode skillKey6;
    public static KeyCode skillKey7;
    public static KeyCode skillKey8;

    private void Start()
    {
        moveRight = KeyCode.RightArrow;
        moveLeft = KeyCode.LeftArrow;
        moveUp = KeyCode.UpArrow;
        moveDown = KeyCode.DownArrow;

        attackKey = KeyCode.Z;
        jumpKey = KeyCode.X;
        collectKey = KeyCode.C;
        evadeKey = KeyCode.Space;
        skillKey1 = KeyCode.A;
        skillKey2 = KeyCode.S;
        skillKey3 = KeyCode.D;
        skillKey4 = KeyCode.F;
        skillKey5 = KeyCode.Q;
        skillKey6 = KeyCode.W;
        skillKey7 = KeyCode.E;
        skillKey8 = KeyCode.R;
    }
}
