using UnityEngine;
using UnityEngine.Events;

public class DebuffControl : Singleton<DebuffControl>
{
    public const string igniteName = "Ignite";

    public void Ignite(Character target, float duration)
    {
        ApplyDebuff(target, igniteName, IgniteEffect, RemoveIgniteEffect, duration);
    }

    private void ApplyDebuff(Character target, string affectName, UnityAction applyAffect, UnityAction removeAffect, float duration)
    {
        target.buffController.AddBuffEvent(affectName, applyAffect, removeAffect, duration);
    }

    #region Ignite
    protected virtual void IgniteEffect()
    {
        // 燃燒 IEnumerator
        Debug.Log("燒");
    }

    protected virtual void RemoveIgniteEffect()
    {

    }
    #endregion
}
