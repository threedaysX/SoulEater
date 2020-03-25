using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public float currentHealth;
    public float currentMana;

    [Header("能力值")]
    public Data data;
    [Header("技能欄")]
    public Skill[] skills;


    private void Awake()
    {
        this.gameObject.AddComponent(typeof(SkillController));
        currentHealth = data.maxHealth.Value;
        Debug.Log(data.resistance.fire.Value);
    }

    public void TakeDamage(float damage, float duration)
    {
        //

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UseSkill(Skill skill)
    {
        Debug.Log(skills[0].skillName);
        Debug.Log(this.data.resistance.fire.Value);
        GetComponent<SkillController>().Trigger(this, skill);
        Debug.Log(this.data.resistance.fire.Value);
    }

    public virtual void Die()
    {

    }
}
