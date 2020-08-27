using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxList : MonoBehaviour
{
    public List<AttackHitbox> attackHitboxes;

    /// <summary>
    /// 取得命中的所有目標
    /// </summary>
    /// <param name="attackNumber">每一輪攻擊中，第N段的攻擊(動畫編號)</param>
    public Collider2D[] GetAttackHits(int attackNumber)
    {
        // 攻擊編號對應到陣列位置要-1
        int index = attackNumber;
        if (index <= 0 || index > attackHitboxes.Count)
            return new Collider2D[0];
        return attackHitboxes[index - 1].GetAttackHits();
    }
}
