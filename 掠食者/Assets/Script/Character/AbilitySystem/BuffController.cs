using UnityEngine;
using UnityEngine.Events;
using System.Collections.Concurrent;

public class BuffContent
{
    // 若未來要加上[流血]、[沉默]...的異常狀態，則要注意這裡(UnityEvent)
    public bool isStartCountDown;
    public UnityEvent affect;
    public UnityEvent removeEvent;
    public float duration;
    public float endTime;

    public void StartTick()
    {
        ResetTick();
        isStartCountDown = true;
    }

    public void ResetTick()
    {
        endTime = Time.time + duration;
    }

    public void StopTick()
    {
        duration = 0;
        endTime = 0;
        isStartCountDown = false;
    }
}

public class BuffController : MonoBehaviour
{
    public ConcurrentDictionary<string, BuffContent> buffList = new ConcurrentDictionary<string, BuffContent>();

    private void Update()
    {
        foreach (var buff in buffList)
        {
            // 若超過Buff時間，則觸發移除效果。
            if (buff.Value.isStartCountDown && Time.time >= buff.Value.endTime)
            {
                if (buff.Value.removeEvent != null)
                {
                    buff.Value.removeEvent.Invoke();
                    buff.Value.StopTick();
                    RemoveMemory(buff.Key);
                }
            }
        }
    }

    #region BuffList設定內容
    private BuffContent CreateMemory(UnityEvent affect, UnityEvent removeEvent, float duration)
    {
        return new BuffContent { affect = affect, removeEvent = removeEvent, duration = duration, endTime = Time.time + duration, isStartCountDown = true };
    }

    private void SetBuffMemory(string name, BuffContent buffMemory)
    {
        buffList.TryAdd(name, buffMemory);
    }

    private BuffContent GetBuffMemory(string name)
    {
        return buffList[name];
    }

    private void RemoveMemory(string name)
    {
        if (!buffList.ContainsKey(name))
            return;

        _ = buffList.TryRemove(name, out _);
    }
    #endregion

    /// <summary>
    /// 附加針對人物的正面、異常效果
    /// </summary>
    /// <param name="affect">持續時間內的影響效果</param>
    /// <param name="removeEvent">當持續時間結束後，觸發的效果</param>
    /// <param name="duration">持續時間</param>
    public void AddBuffEvent(string affectName, UnityAction affect, UnityAction removeEvent, float duration)
    {
        string name = affectName;
        if (!CheckIsBuffInList(affectName))
        {
            if (affect != null)
            {
                affect.Invoke();
            }
            SetBuffMemory(name, CreateMemory(CreateAffectEvent(affect), CreateAffectEvent(removeEvent), duration));
        }
        else
        {
            // 重置時間
            GetBuffMemory(name).ResetTick();
        }
    }

    /// <summary>
    /// 檢查Buff是否已經存在，存在則回傳True。(相同的Buff加成只能有一個)
    /// </summary>
    private bool CheckIsBuffInList(string name)
    {
        if (buffList.ContainsKey(name))
            return true;
        return false;
    }

    private UnityEvent CreateAffectEvent(UnityAction call)
    {
        UnityEvent affect = new UnityEvent();
        affect.AddListener(call);

        return affect;
    }
}
