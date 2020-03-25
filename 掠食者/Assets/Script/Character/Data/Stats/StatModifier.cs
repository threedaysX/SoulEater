namespace StatsModifierModel
{
    public enum StatModType
    {
        FlatAdd,        // 單純相加 (+1力量 & +2力量 => +3力量、+20%火炕...)
        Times,          // [倍]數相乘 (+1倍 & +1倍 => 4倍 => 2*2)
        TimesOfAdd,     // [倍]數相加 (+1倍 & +1倍 => 2倍)
        PercentMult,    // [倍]率相乘 (+60% & +60% => 256% => 1.6*1.6)
        PercentAdd      // [倍]率相加 (+50% & +60% => 110%)
    }

    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;

        public StatModifier(float value, StatModType type)
        {
            Value = value;
            Type = type;
        }
    }
}
