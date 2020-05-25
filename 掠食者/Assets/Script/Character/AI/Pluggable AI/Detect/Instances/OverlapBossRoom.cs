using UnityEngine;

[CreateAssetMenu(menuName =("Character/AI/Detect/DetectBossRoom"))]
public class OverlapBossRoom : Detect
{
    [SerializeField] private Vector2 topLeft;
    [SerializeField]private Vector2 rightBottom;
    public override bool StartDetectHaviour()
    {
        return DetectBossRoom();
    }

    private bool DetectBossRoom()
    {
        Collider2D[] cols = Physics2D.OverlapAreaAll(topLeft, rightBottom);

        foreach(Collider2D col in cols)
        {
            if (!col)
            {

            }
            else if (col.CompareTag("Player"))
            {
                ai.chaseTarget = col.transform;
                return true;
            }
        }
        return false;
    }
}
