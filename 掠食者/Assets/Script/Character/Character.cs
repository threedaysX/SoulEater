using StatsModel;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("基礎")]
    public string characterName;
    public float currentHealth;
    public float currentMana;

    [Header("操作")]
    public bool canMove = true;
    public bool canJump = true;
    public bool canEvade = true;
    public bool canAttack = true;
    public bool canSkill = true;

    [Header("能力值")]
    public Data data;
    [Header("技能欄")]
    public Skill[] skills;

    [HideInInspector] public SkillController skillController;
    [HideInInspector] public BuffController buffController;

    private void Awake()
    {
        this.gameObject.AddComponent(typeof(SkillController));
        this.gameObject.AddComponent(typeof(BuffController));
        skillController = GetComponent<SkillController>();
        buffController = GetComponent<BuffController>();

        currentHealth = data.maxHealth.Value;
        currentMana = data.mana.Value;
    }   

    public void TakeDamage(float damage, float timesOfPerDamage = 0, float duration = 0)
    {
        if (timesOfPerDamage <= 0 || duration <= 0)
        {
            currentHealth -= damage;
            Debug.Log(characterName + "受到 " + damage + "點傷害");
        }
        else
        {
            StartCoroutine(TakeDamagePerSecondInTimes(damage, timesOfPerDamage, duration));
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator TakeDamagePerSecondInTimes(float damage, float timesOfPerDamage, float duration)
    {
        while(duration > 0)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(timesOfPerDamage);
            duration -= timesOfPerDamage;
        }
        yield break;
    }

    public void UseSkill(Skill skill)
    {
        if (!canSkill)
            return;

        Debug.Log(skills[0].skillName);
        Debug.Log(this.data.resistance.fire.Value);
        skillController.Trigger(skill);
        Debug.Log(this.data.resistance.fire.Value);
    }

    public virtual void Die()
    {

    }

    public float GetSkillCost(CostType costType)
    {
        float resultCost = currentMana;
        switch (costType)
        {
            case CostType.Health:
                resultCost = currentHealth;
                break;
            case CostType.Mana:
                break;
        }

        return resultCost;
    }

    public Stats GetAttack(SkillType skillType)
    {
        Stats resultAttack = data.attack;
        switch (skillType)
        {
            case SkillType.Attack:
                break;
            case SkillType.MagicAttack:
                resultAttack = data.magicAttack;
                break;
        }

        return resultAttack;
    }

    public Stats GetResistance(ElementType elementType)
    {
        Stats resultResistance = data.resistance.none;
        switch (elementType)
        {
            case ElementType.None:
                break;
            case ElementType.Fire:
                resultResistance = data.resistance.fire;
                break;
            case ElementType.Water:
                resultResistance = data.resistance.water;
                break;
            case ElementType.Earth:
                resultResistance = data.resistance.earth;
                break;
            case ElementType.Air:
                resultResistance = data.resistance.air;
                break;
            case ElementType.Thunder:
                resultResistance = data.resistance.thunder;
                break;
            case ElementType.Light:
                resultResistance = data.resistance.light;
                break;
            case ElementType.Dark:
                resultResistance = data.resistance.dark;
                break;
        }

        return resultResistance;
    }
}
