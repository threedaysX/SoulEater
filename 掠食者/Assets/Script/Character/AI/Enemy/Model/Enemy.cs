using UnityEngine;

public class Enemy : AI
{
    [Header("階級")]
    public EnemyLevel enemyLevel;

    public override void Start()
    {
        base.Start();

        this.tag = "Enemy";
        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    public override void Update()
    {
        base.Update();

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
        ResetBarUI();
        base.Die();
    }

    public void GetUnlimitedMana()
    {
        this.data.maxMana.AddModifier(new StatsModifierModel.StatModifier(1000000, StatModType.FlatAdd, "UnlimitedMana"));
    }

    public void SetEnemyLevel(EnemyLevel level)
    {
        enemyLevel = level;
    }
}

public enum EnemyLevel 
{
    Boss,
    Normal
}

