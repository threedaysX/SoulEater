using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyModel : AI
{
    [Header("階級")]
    public EnemyLevel enemyLevel;

    [Header("動作前置提示")]
    public AudioClip preActSound;
    public ParticleSystem preActHint;

    public override void Start()
    {
        base.Start();

        tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        ResetBarUI();

        // 外力因素影響會無法行動(異常狀態、使用技能等)
        if (isLockAction && CanAction)
        {
            CanAction = false;
        }

        if (!isLockAction && !CanAction)
        {
            CanAction = true;
        }
    }

    private void ResetBarUI()
    {
        if (isHealthDirty)
        {
            EnemyUIControl.Instance.SetHealthUI(characterName, data.maxHealth.Value, CurrentHealth);
            isHealthDirty = false;
        }
    }

    public override void Die()
    {
        base.Die();
        ResetBarUI();
    }

    public void SetEnemyLevel(EnemyLevel level)
    {
        enemyLevel = level;
    }

    protected virtual void DoPreActHint(ParticleSystem overridedPreActHintEffect = null, AudioClip overridedPreActSound = null)
    {
        if (overridedPreActHintEffect != null)
        {
            overridedPreActHintEffect.Play(true);
        }
        else if (preActHint != null)
        {
            preActHint.Play(true);
        }
        if (overridedPreActSound != null)
        {
            operationSoundController.PlaySound(overridedPreActSound);
        }
        else if (preActSound != null)
        {
            operationSoundController.PlaySound(preActSound);
        }
    }
}

public enum EnemyLevel 
{
    Boss,
    Normal
}

