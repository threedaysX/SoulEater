using StatsModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 詞綴
/// </summary>
public class Affix : MonoBehaviour, IAffixTrigger
{
    public string description;
    public UnityEvent affect;

    public void TriggerAffixOnTouch()
    {

    }

    public void ApplyStats(Character character, Stats stats)
    {

    }

    public void AddAffect(UnityAction effect)
    {
        affect.AddListener(effect);
    }

    public void InvokeAffect()
    {
        affect.Invoke();
    }
}
