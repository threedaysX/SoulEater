
public enum AttackType
{
    Attack,
    MagicAttack,
    Effect
}

public enum CostType
{
    Health,
    Mana
}

public enum ElementType
{
    None,
    Fire,
    Water,
    Earth,
    Air,
    Thunder,
    Light,
    Dark
}

public enum StatModType
{
    FlatAdd,          // 單純相加 (+1力量 & +2力量 => +3力量、+20%火炕...)
    Times,            // [倍]數相乘 (+1倍 & +1倍 => 4倍 => 2*2)
    TimesOfAdd,       // [倍]數相加 (+1倍 & +1倍 => 2倍)
    Magnification,    // [倍]率相乘 (+60% & +60% => 256% => 1.6*1.6)
    MagnificationAdd  // [倍]率相加 (+50% & +60% => 110%)
}