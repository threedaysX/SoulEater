using UnityEngine;

public class Player : Character
{
    [Header("死亡特效")]
    public ParticleSystem dieParticle;

    [Header("血量UI震動")]
    public UIShake healthUI;
    private PlayerSkillSlotKeyControl skillKey;

    private void Start()
    {
        this.tag = "Player";
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        skillKey = GetComponent<PlayerSkillSlotKeyControl>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(HotKeyController.attackKey1) || Input.GetKeyDown(HotKeyController.attackKey2))
        {
            StartAttack(AttackType.Attack, data.attackElement);
        }

        if (Input.GetKeyDown(HotKeyController.skillKey1))
        {
            UseSkill(skillKey.SkillSlot1.skill);
        }
        if (Input.GetKeyDown(HotKeyController.skillKey2))
        {
            UseSkill(skillKey.SkillSlot2.skill);
        }
        if (Input.GetKeyDown(HotKeyController.skillKey3))
        {
            UseSkill(skillKey.SkillSlot3.skill);
        }
        if (Input.GetKeyDown(HotKeyController.skillKey4))
        {
            UseSkill(skillKey.SkillSlot4.skill);
        }
        if (Input.GetKeyDown(HotKeyController.skillKey5))
        {

        }
        if (Input.GetKeyDown(HotKeyController.skillKey6))
        {

        }
        if (Input.GetKeyDown(HotKeyController.skillKey7))
        {

        }
        if (Input.GetKeyDown(HotKeyController.skillKey8))
        {

        }

        ResetBarUI();
    }

    public override bool TakeDamage(int damage, bool isCritical, float damageDirectionX = 0, float weaponKnockBackForce = 0, float timesOfPerDamage = 0, float duration = 0, bool damageImmediate = true)
    {
        bool isDamaged = base.TakeDamage(damage, isCritical, damageDirectionX, weaponKnockBackForce, timesOfPerDamage, duration, damageImmediate);
        if (isDamaged)
        {
            if (CurrentHealth <= data.maxHealth.Value * 0.25f)
            {
                healthUI.Shake(0.12f);
            }
            else if (CurrentHealth <= data.maxHealth.Value * 0.5f)
            {
                healthUI.Shake(0.08f);
            }
            else if (CurrentHealth <= data.maxHealth.Value * 0.75f)
            {
                healthUI.Shake(0.04f);
            }
        }
        return isDamaged;
    }

    public override void Die()
    {
        ResetBarUI();
        SlowMotionController.Instance.DoSlowMotion(0.05f, dieController.dieDuration);
        StartCoroutine(FadeScreen.Instance.Fade(8f, 8f));
        base.Die();
    }

    private void ResetBarUI()
    {
        if (isHealthDirty)
        {
            PlayerUIControl.Instance.SetHealthUI(data.maxHealth.Value, CurrentHealth);
            isHealthDirty = false;
        }

        if (isManaDirty)
        {
            PlayerUIControl.Instance.SetManaUI(data.maxMana.Value, CurrentMana);
            isManaDirty = false;
        }
    }
}
