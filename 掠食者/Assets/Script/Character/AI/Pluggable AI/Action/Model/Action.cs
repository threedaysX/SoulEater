using UnityEngine;

// 複製用: 
// [CreateAssetMenu(menuName = "Character/AI/Action/繼承的新ClassName")]
public abstract class Action : AIHaviourBase
{
    // 此動作的權重
    public int actionWeight;
    public ActionType actionType;
    public Animator actionAnimator;
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

public enum ActionType 
{
    Idle,
    FackAction, // 不做任何實際行動，類似威嚇、咆嘯、示威...等動作
    Move,
    Retreat,
    Attack,
    MagicAttack,
    Effect,
}

