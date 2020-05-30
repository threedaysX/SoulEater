namespace StatsModifierModel
{
    [System.Serializable]
    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly string SourceName;

        public StatModifier(float value, StatModType type, string sourceName)
        {
            Value = value;
            Type = type;
            SourceName = sourceName;
        }

        public StatModifier(float value, StatModType type) : this(value, type, string.Empty) { }
    }

}

public enum StatModType
{
    /// <summary>
    /// 單純相加 (+1力量 & +2力量 => +3力量、+20%火抗...)
    /// </summary>
    FlatAdd,

    /// <summary>
    /// [倍]數相乘 (+1倍 & +1倍 => 4倍 => 2*2)
    /// </summary>
    Times,

    /// <summary>
    /// [倍]數相加 (+1倍 & +1倍 => 2倍)
    /// </summary>
    TimesOfAdd,

    /// <summary>
    /// [倍]率相乘 (+60% & +60% => 256% => 1.6*1.6)
    /// </summary>
    Magnification,

    /// <summary>
    /// [倍]率相加 (+50% & +60% => 110%)
    /// </summary>
    MagnificationAdd
}