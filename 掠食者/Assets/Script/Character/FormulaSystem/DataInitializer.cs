using System;

public class DataInitializer
{
    public Status status;
    private float Str { get; set; }
    private float Agi { get; set; }
    private float Vit { get; set; }
    private float Dex { get; set; }
    private float Int { get; set; }
    private float Luk { get; set; }


    public DataInitializer(Status status)
    {
        this.status = status;
        Str = status.strength.Value;
        Agi = status.agility.Value;
        Vit = status.vitality.Value;
        Dex = status.dexterity.Value;
        Int = status.intelligence.Value;
        Luk = status.lucky.Value;
    }

    public float GetMaxHealth()
    {
        return (float)(Math.Round(((10 + Vit + Str / 5) * Vit * (1 + Vit / 50 + Vit / 100)) + (Vit * Vit / 5 * Vit / 10) + 400, 0) + Math.Round(10 * (Str / 2) * (Str / 5), 0));
    }

    public float GetMaxMana()
    {
        return (float)(Math.Round(3 + Int / 5 + 4 * (Int / 10) * (Int / 100), 0)); 
    }

    public float GetAttack()
    {
        return (float)(Math.Round(10 + Str / 2 + 2 * (Str / 5) * (Str / 10) + (Str * Str / 20), 0) + Math.Round(1.8 * Dex, 0) + Math.Round(0.8 * Luk, 0));
    }

    public float GetMagicAttack()
    {
        return (float)(Math.Round(2 * (Int + Int / 10) + Int * Int / (10 - 5 / Int), 0) + Math.Round(1.2 * Luk, 0));
    }

    public float GetDefense()
    {
        return (float)(Math.Max(Math.Round((100 + Vit / 2 + Str / 2 - 50 * (20 / (Vit)) * (1 + (Vit / 50) * (Vit / 100) * (Str / 50))) * 0.5, 0), 1));
    }

    public float GetCritical()
    {
        return (float)(1.6 * Luk);
    }

    public float GetAttackSpeed()
    {
        return (float)(Math.Round(0.8 + 0.24 * Agi, 2));
    }

    public float GetMoveSpeed()
    {
        // 無條件捨去到小數1位 => 乘以 10 再除以 10
        var movePoint = Math.Floor((1 + 0.2 * (Agi / 15)) * 0.1 * 100)  / 10;
        return (float)Math.Min(movePoint, 1.5);
    }

    public float GetJumpForce()
    {
        // 無條件捨去到小數1位 => 乘以 10 再除以 10 
        var jumpForcePoint = Math.Floor((1 + 0.2 * (Str / 15)) * 0.1 * 100) / 10;
        return (float)jumpForcePoint;
    }

    public float GetKnockbackDamage()
    {
        float health = GetMaxHealth();
        return (float)(Math.Round(health / 10 + health / (10 * (health / Vit) / (health / 8)), 0));
    }

    public float GetManaRecoveringOfDamage()
    {
        return (float)(Math.Max(Math.Min(Math.Round(108 - 1.6 * Int - 0.8 * (Int / 5) * (Int / 20), 0), 100), 1));
    }

    public float GetSkillCoolDownReduce()
    {
        return (float)(Math.Min(0.8 * Dex, 40));
    }

    public float GetCastTimeReduce()
    {
        return (float)(Math.Min(Dex, 50) + Math.Max(Math.Round(30 - 15 * (10 / Int), 1), 0));
    }

    public float GetEvadeCoolDownReduce()
    {
        return (float)(Math.Min(Math.Round(1 * Agi + Math.Pow(1.05, Agi) * (Agi / 10), 2), 80));
    }
}
