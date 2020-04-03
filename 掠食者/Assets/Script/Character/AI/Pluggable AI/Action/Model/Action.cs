using UnityEngine;

// 複製用: 
// [CreateAssetMenu(menuName = "Character/AI/Action/繼承的新ClassName")]
public abstract class Action : AIHaviourBase
{
    /// <summary>
    /// 此動作的權重，基本以每2點為一個階段。
    /// [0~1] 不太想
    /// [2~3] 較少
    /// [4~5] 普通
    /// [6~7] 常採取的動作
    /// [8~9] 擅長而且喜歡選擇的動作
    /// [10] 超想這麼做
    /// </summary>
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
                if (judje.actionWeightAfterJudge != 0)
                    actionWeight = judje.actionWeightAfterJudge;
                return true;
            }
        }
        return false;
    }

    public abstract bool StartActHaviour();
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

