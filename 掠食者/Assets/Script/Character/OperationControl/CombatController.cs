using UnityEngine;
using System.Linq;
using System.Collections;

public class CombatController : MonoBehaviour
{
    private Character character;
    public bool hasHit = false;  //AI用
    public float hasHitInTime;  //AI用
    public float takeHowMuchDamage;  //AI用

    [SerializeField] private float attackPointBasicRange = 1f;

    private void Start()
    {
        character = GetComponent<Character>();
        RenderAttackHitboxes(true);
        RenderHitEffect(true);
    }

    public bool Attack(AttackType attackType = AttackType.Attack, ElementType elementType = ElementType.None)
    {
        bool attackSuccess = false;
        Collider2D[] hits = DrawAttackRange();
        foreach (Collider2D target in hits)
        {
            if (target.CompareTag("Untagged"))
                continue;

            // 不會打到自己人
            if (target != null && !target.CompareTag(character.tag))
            {
                var enemyDetails = target.gameObject.GetComponent<Character>();
                if (enemyDetails != null && !enemyDetails.GetImmuneState())
                {
                    // 重新確認目標位置
                    if (CheckIsTargetStillInAttackRange(target))
                    {
                        // 取得傷害來源方向(KB擊退用途)
                        float damageDirectionX = character.transform.position.x - target.transform.position.x;
                        float damage = DamageController.Instance.GetAttackDamage(character, enemyDetails, attackType, elementType);
                        enemyDetails.TakeDamage((int)damage, damageDirectionX, character.data.weaponKnockBackForce);
                        character.DamageDealtSteal(damage, true);
                        TriggerHitEffect(target.transform);
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
        AttackHitboxList hitboxes = character.transform.GetComponentInChildren<AttackHitboxList>(true);
        if (hitboxes != null)
        {
            int attackOrder = character.operationController.AttackAnimNumber;
            hitboxes.gameObject.SetActive(true);
            Collider2D[] hits = hitboxes.GetAttackHits(attackOrder);
            hitboxes.gameObject.SetActive(false);
            return hits;
        }
        return Physics2D.OverlapCircleAll(GetAttackPoint(), character.data.attackRange.Value);
    }

    public void TriggerHitEffect(Transform target)
    {
        ParticleSystem hitEffect = character.transform.GetComponentInChildren<ParticleSystem>();
        if (hitEffect != null)
        {
            hitEffect.transform.position = target.position;
            hitEffect.Emit(1);
        }
    }

    public void RenderAttackHitboxes(bool reRenderOnce = false)
    {
        PrefabRenderer.Instance.RenderPrefabInParent<AttackHitboxList>(character.transform, character.data.attackHitBoxPrefab, "AttackHitboxes", false, reRenderOnce);
    }

    public void RenderHitEffect(bool reRenderOnce = false)
    {
        PrefabRenderer.Instance.RenderPrefabInParent<ParticleSystem>(character.transform, character.data.hitEffectPrefab, "hitEffect", true, reRenderOnce);
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
