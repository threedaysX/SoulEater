using UnityEngine;

public class Combat : MonoBehaviour
{
    public float nextAttackTime;
    private Character character;

    [SerializeField] private float attackPointBasicRange = 1f;

    private void Start()
    {
        nextAttackTime = 0;
        character = GetComponent<Character>();
    }

    public bool StartAttack(AttackType attackType = AttackType.Attack, ElementType elementType = ElementType.None)
    {
        bool attackSuccess = false;
        // Attack
        if (Time.time >= nextAttackTime)
        {
            attackSuccess = Attack(attackType, elementType);
            nextAttackTime = Time.time + 1 / character.data.attackSpeed.Value;
        }

        return attackSuccess;
    }

    private bool Attack(AttackType attackType = AttackType.Attack, ElementType elementType = ElementType.None)
    {
        bool attackSuccess = false;
        Collider2D[] hit = Physics2D.OverlapCircleAll(GetAttackPoint(), character.data.attackRange.Value);

        foreach (Collider2D enemy in hit)
        {
            Debug.Log(enemy);
            // 不會打到自己人
            if (enemy != null && !enemy.CompareTag(character.tag))
            {
                var enemyDetails = enemy.gameObject.GetComponent<Character>();
                if (enemyDetails == null)
                    return attackSuccess;

                enemyDetails.TakeDamage(DamageController.Instance.GetAttackDamage(character, enemyDetails, attackType, elementType));
                attackSuccess = true;
            }
        }

        return attackSuccess;
    }

    private void OnDrawGizmos()
    {
        if (character == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetAttackPoint(), character.data.attackRange.Value);
    }

    private Vector2 GetAttackPoint()
    {
        return character.transform.position + character.transform.right * attackPointBasicRange;
    }
}
