using UnityEngine;

// 複製用: 
// [CreateAssetMenu(menuName = "Character/AI/Action/繼承的新ClassName")]
public abstract class Action : AIHaviourBase
{
    // 此動作的權重
    public int actionWeight;
    public Animation actionAnimation;
    public Judgement[] judjements;

    public bool CheckActionThatCanDo()
    {
        foreach (Judgement judje in judjements)
        {
            if (judje == null)
                continue;
            judje.GetCurrentAI(ai);
            judje.StartCheckActCondition();     // 開始檢查該動作的各個觸發條件
            if (judje.CheckTrueConditionCount())
            {
                return true;
            }
        }
        return false;
    }

    public abstract void StartActHaviour();
}
