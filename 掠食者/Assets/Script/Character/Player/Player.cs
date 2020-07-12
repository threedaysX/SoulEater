using UnityEngine;

[RequireComponent(typeof(PlayerSkillSlotKeyControl))]
[RequireComponent(typeof(PlayerMovement))]
public class Player : Character
{
    [Header("死亡特效")]
    public ParticleSystem dieParticle;

    [Header("血量UI震動")]
    public UIShake healthUI;

    private void Start()
    {
        this.tag = "Player";
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.AttackKey1)) || Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.AttackKey2)))
        {
            StartAttack(AttackType.Attack, data.attackElement);
        }

        ResetBarUI();
    }

    public override bool TakeDamage(GameObject damageSource, int damage, bool isCritical, float damageDirectionX = 0, float weaponKnockBackForce = 0, float timesOfPerDamage = 0, float duration = 0, bool damageImmediate = true)
    {
        bool isDamaged = base.TakeDamage(damageSource, damage, isCritical, damageDirectionX, weaponKnockBackForce, timesOfPerDamage, duration, damageImmediate);
        if (isDamaged)
        {
            if (CurrentHealth <= data.maxHealth.Value * 0.25f)
            {
                healthUI.Shake(0.15f);
            }
            else if (CurrentHealth <= data.maxHealth.Value * 0.5f)
            {
                healthUI.Shake(0.12f);
            }
            else if (CurrentHealth <= data.maxHealth.Value * 0.75f)
            {
                healthUI.Shake(0.08f);
            }
        }
        return isDamaged;
    }

    public override void Die()
    {
        ResetBarUI();
        TimeScaleController.Instance.DoSlowMotion(0.05f, dieController.dieDuration);
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
