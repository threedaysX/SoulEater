using UnityEngine;
using System.Collections;
using StatsModifierModel;
using StatsModel;

public class BuffController : Singleton<BuffController>
{
    /// <summary>
    /// 增、減能力。
    /// </summary>
    /// <param name="mod">增加的數值</param>
    /// <param name="duration">Buff持續多久的時間</param>
    public void AddModifier(Stats stats, StatModifier mod, float duration)
    {
        StartCoroutine(AddModifierContent(stats, mod, duration));
    }

    private IEnumerator AddModifierContent(Stats stats, StatModifier mod, float duration)
    {
        if (mod.Value != 0)
        {
            stats.AddModifier(mod);
        }
        if (duration == 0)
            yield break;

        yield return new WaitForSeconds(duration);
        RemoveModifier(stats, mod);    // 時間到就移除效果
    }

    public bool RemoveModifier(Stats stats, StatModifier mod)
    {
        if (mod.Value != 0)
        {
            if (stats.RemoveModifier(mod))
            {
                return true;
            }
        }
        return false;
    }
}
