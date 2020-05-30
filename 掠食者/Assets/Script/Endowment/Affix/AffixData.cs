using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 詞綴效果 (Prefab)
/// </summary>
public abstract class AffixData : MonoBehaviour
{
    public Character character; // 誰擁有這個詞綴效果
    protected bool startTrigger;    // 是否被觸發

    [Header("立即效果")]
    public UnityEvent immediatelyAffect;    // 當詞綴被觸發時，立即觸發的效果 (Ex: 能力增減、獲得技能)
    [Header("持續效果")]
    public UnityEvent persistentAffect;     // 當詞綴被觸發後，持續判斷並觸發的效果 (Ex: n%機率觸發)

    protected virtual void Update()
    {
        if (startTrigger)
        {
            TriggerPersistentAffect();
        }
    }

    public void Trigger(bool start)
    {
        startTrigger = start;
    }

    /// <summary>
    /// 觸發立即性效果
    /// </summary>
    protected void TriggerImmdeiatelyAffext()
    {
        if (character != null)
            immediatelyAffect.Invoke();
    }

    /// <summary>
    /// 碎片持續觸發的效果
    /// </summary>
    protected void TriggerPersistentAffect()
    {
        if (character != null)
            persistentAffect.Invoke();
    }

    protected abstract void AddAffectEvents();
}
