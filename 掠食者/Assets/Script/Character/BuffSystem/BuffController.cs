using UnityEngine;
using System.Collections;
using StatsModifierModel;
using StatsModel;

public class BuffController : MonoBehaviour
{
    /// <summary>
    /// 增、減能力。
    /// </summary>
    /// <param name="mod">增加的數值</param>
    /// <param name="duration">Buff持續多久的時間</param>
    public void AddModifier(Stats stat, StatModifier mod, float duration)
    {
        if (CheckModifier(stat, mod))
        {
            StartCoroutine(AddModifierContent(stat, mod, duration));
        }
        else
        {
            //StopAllCoroutines(); 去除舊的，並重置Buff時間
            RemoveModifier(stat, mod);
            StartCoroutine(AddModifierContent(stat, mod, duration));
        }
    }

    private IEnumerator AddModifierContent(Stats stat, StatModifier mod, float duration)
    {
        if (mod.Value != 0)
        {
            stat.AddModifier(mod);
        }
        if (duration == 0)
            yield break;

        Debug.Log(Time.time);
        yield return new WaitForSeconds(duration);
        Debug.Log("A" + Time.time);
        RemoveModifier(stat, mod);    // 時間到就移除效果
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

    /// <summary>
    /// 相同的Buff加成只能有一個
    /// </summary>
    private bool CheckModifier(Stats stat, StatModifier newMod)
    {
        foreach (var mod in stat.modifiers)
        {
            if (newMod.SourceName.ToString() == "烈焰鎧甲")
            {
                return false;
            }
        }
        return true;
    }
}
