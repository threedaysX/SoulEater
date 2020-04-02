namespace StatsModifierModel
{
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
