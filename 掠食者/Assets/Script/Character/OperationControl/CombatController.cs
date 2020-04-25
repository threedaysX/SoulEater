using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    private Character character;
    private List<Collider2D> hitAreaList;
    public bool hasHit = false;  //AI用
    public float hasHitInTime;  //AI用
    public float takeHowMuchDamage;  //AI用

    [SerializeField] private float attackPointBasicRange = 1f;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    public bool Attack(AttackType attackType = AttackType.Attack, ElementType elementType = ElementType.None)
    {
        bool attackSuccess = false;
        Collider2D[] hit = DrawAttackRange();

        foreach (Collider2D target in hit)
        {
            Debug.Log("攻擊的目標:" + target);
            // 不會打到自己人
            if (target != null && !target.CompareTag(character.tag))
            {
                var enemyDetails = target.gameObject.GetComponent<Character>();
                if (enemyDetails != null)
                {
                    // 重新確認目標位置
                    if (CheckIsTargetStillInAttackRange(target))
                    {
                        enemyDetails.TakeDamage(DamageController.Instance.GetAttackDamage(character, enemyDetails, attackType, elementType));
                        attackSuccess = true;
                        StartCoroutine(HasHit());
                    }
                }

            }
        }

        return attackSuccess;
    }

    private bool CheckIsTargetStillInAttackRange(Collider2D target)
    {
        Collider2D[] reDrawTarget = DrawAttackRange();
        if (reDrawTarget.Contains(target))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 根據武器調整繪製方式
    /// </summary>
    /// <returns></returns>
    private Collider2D[] DrawAttackRange()
    {
        return Physics2D.OverlapCircleAll(GetAttackPoint(), character.data.attackRange.Value);
    }

    private Vector2 GetAttackPoint()
    {
        return character.transform.position + character.transform.right * attackPointBasicRange;
    }

    private IEnumerator HasHit()
    {
        hasHit = true;
        yield return new WaitForSeconds(hasHitInTime);
        hasHit = false;
        yield break;
    }

    private void OnDrawGizmos()
    {
        if (character == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetAttackPoint(), character.data.attackRange.Value);
    }
}
