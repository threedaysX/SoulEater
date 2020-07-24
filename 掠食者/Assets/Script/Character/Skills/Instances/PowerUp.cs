using StatsModifierModel;
using StatsModel;

public class PowerUp : DisposableSkill
{
    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(BuffStr);
        immediatelyAffect.AddListener(BuffAgi);
        immediatelyAffect.AddListener(BuffDex);
    }

    public string strBuffName = "STR上升";
    /// <summary>
    /// 微幅增加自身力量，持續6秒
    /// </summary>
    private void BuffStr()
    {
        var str = sourceCaster.data.status.strength;
        BuffStat(str, strBuffName, 6f);
    }

    public string agiBuffName = "AGI上升";
    /// <summary>
    /// "微幅增加自身敏捷屬性，持續6秒
    /// </summary>
    private void BuffAgi()
    {
        var agi = sourceCaster.data.status.agility;
        BuffStat(agi, agiBuffName, 6f);
    }

    public string dexBuffName = "DEX上升";
    /// <summary>
    /// "微幅增加自身靈巧屬性，持續6秒
    /// </summary>
    private void BuffDex()
    {
        var dex = sourceCaster.data.status.dexterity;
        BuffStat(dex, dexBuffName, 6f);
    }

    private void BuffStat(Stats stats, string buffName, float duration)
    {
        int offset = 0;
        if (stats.Value < 20)
            offset += 5;
        else
            offset += 2;

        void affect()
        {
            stats.AddModifier(new StatModifier(offset, StatModType.FlatAdd, buffName));
        }
        void remove()
        {
            stats.RemoveModifier(new StatModifier(-offset, StatModType.FlatAdd, buffName));
        }
        sourceCaster.buffController.AddBuff(buffName, affect, remove, duration);
    }
}

