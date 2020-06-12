using StatsModifierModel;
using System.Collections;
using UnityEngine;

/// <summary>
/// 伊夫利特
/// </summary>
public class Ifrit : EnemyModel
{
    [Header("血量UI震動")]
    public UIShake healthUI;

    [Header("攻擊速度(延遲)")]
    [Range(0f, 1f)]
    public float forceAttackDelay;

    [Header("型態改變")]
    public AudioClip typeChangingSound;
    public AudioClip typeChangedBurstSound;
    public ParticleSystem typeChangingEffect;
    public float typeChangeDuration;
    private bool flamethrowerTypeChanged;

    public override void Start()
    {
        base.Start();
        SetEnemyLevel(EnemyLevel.Boss);
        ResetFlamethrowerData();
        ForceAdjustAttackDelay();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        FlamethrowerTypeChange();
        NaraBurstTypeChange();
    }

    protected void ForceAdjustAttackDelay()
    {
        data.attackDelay.ForceToChangeValue(forceAttackDelay);
    }

    protected void NaraBurstTypeChange()
    {
        // 當血量小於30%，讓Burst爆裂更快速
    }

    private void ResetFlamethrowerData()
    {
        flamethrowerTypeChanged = false;
    }

    protected void FlamethrowerTypeChange()
    {
        // 當血量小於50%，讓噴射詠唱更短
        if (!flamethrowerTypeChanged && CurrentHealth <= data.maxHealth.Value * 0.5)
        {
            GetSkillByName(SkillNameDictionary.flamethrower).castTime.AddModifier(new StatModifier(-50, StatModType.Magnification));
            flamethrowerTypeChanged = true;
        }
    }

    /// <summary>
    /// Use AiAction to Call.
    /// </summary>
    /// <returns></returns>
    public void ChangeType()
    {
        StartCoroutine(TypeChange());
    }
    private IEnumerator TypeChange()
    {
        this.CanAction = false;
        GetIntoImmune(typeChangeDuration);
        CameraShake.Instance.ShakeCamera(1f, 10f, typeChangeDuration, true);
        typeChangingEffect.Play();
        operationSoundController.PlaySound(typeChangingSound);

        yield return new WaitForSeconds(typeChangeDuration);

        operationSoundController.PlaySound(typeChangedBurstSound);
        UseSkill(GetSkillByName(SkillNameDictionary.shockWave), true, true);
        this.CanAction = true;
    }

    public override bool TakeDamage(int damage, bool isCritical, float damageDirectionX = 0, float weaponKnockBackForce = 0, float timesOfPerDamage = 0, float duration = 0, bool damageImmediate = true)
    {
        bool isDamaged = base.TakeDamage(damage, isCritical, damageDirectionX, weaponKnockBackForce, timesOfPerDamage, duration, damageImmediate);
        if (isDamaged && healthUI != null)
        {
            // 根據血量調整震動幅度
            if (CurrentHealth <= data.maxHealth.Value * 0.25f)
            {
                healthUI.Shake(0.15f);
            }
            else if (CurrentHealth <= data.maxHealth.Value * 0.5f)
            {
                healthUI.Shake(0.1f);
            }
            else if (CurrentHealth <= data.maxHealth.Value * 0.75f)
            {
                healthUI.Shake(0.06f);
            }
        }
        return isDamaged;
    }

    public override bool StartAttack(AttackType attackType = AttackType.Attack, ElementType elementType = ElementType.None)
    {
        if (attack.canDo)
        {
            DoPreActHint();
        }
        return base.StartAttack(attackType, elementType);
    }

    public override void Die()
    {
        CameraShake.Instance.ShakeCamera(4f, 8f, dieController.dieDuration, true);
        base.Die();
    }
}