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
    public int originalActionWeight;
    private int _ActionWeight;
    public int ActionWeight { 
        get 
        {
            return _ActionWeight;
        } 
        set 
        {
            diffCount += (_ActionWeight - value);
            _ActionWeight = value;
        }
    }
    private int diffCount; // 這個行動已經被降低過了N點權重
    public ActionType actionType;
    public Animator actionAnimator;
    public Judgement[] judjements;

    public bool CheckActionThatCanDo()
    {
        // 若權重已經小於原始權重的一半，則重置權重。
        if (ActionWeight < originalActionWeight / 2)
        {
            ActionWeight = originalActionWeight;
            diffCount = 0;
        }
        foreach (Judgement judje in judjements)
        {
            if (judje == null)
                continue;
            judje.GetCurrentAI(ai);
            judje.StartCheckActCondition();     // 開始檢查該動作的各個觸發條件
            if (judje.CheckTrueConditionCount())
            {
                // 將判斷後的權重設在動作權重上
                if (judje.actionWeightAfterJudge != 0)
                    ActionWeight = judje.actionWeightAfterJudge - diffCount;
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

