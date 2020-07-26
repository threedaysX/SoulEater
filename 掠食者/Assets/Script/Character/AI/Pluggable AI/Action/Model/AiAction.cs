using UnityEngine;

// 複製用: 
// [CreateAssetMenu(menuName = "Character/AI/Action/繼承的新ClassName")]
public abstract class AiAction : AiHaviourBase
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
    public float ActionWeight;

    /// <summary>
    /// 這個行動已經被降低過了N點權重
    /// </summary>
    public float DiffCount
    {
        get
        {
            return _diffCount;
        }
        set
        {
            _diffCount = value;
            if (_diffCount < 0)
                _diffCount = 0;
        }
    }

    public float originalActionWeight;

    [Header("行為延遲")]
    public float commonActionDelay;
    public AnimationClip[] clips;

    [Header("權重減值")]
    public float minusWeightAmountWhenNotSuccess = 2;
    public float minusWeightAmountAfterAction = 1;
    [SerializeField] private float _diffCount;

    [Header("行為")]
    public AiActionType actionType;
    public Judgement[] judjements;

    public bool CheckActionThatCanDo()
    {
        // 若權重已經小於0，則重置權重。
        if (ActionWeight < 0)
        {
            ResetWeight();
        }
        foreach (Judgement judje in judjements)
        {
            if (judje == null)
                continue;
            judje.GetCurrentAIHavior(ai);
            judje.StartCheckActCondition();     // 開始檢查該動作的各個觸發條件
            if (judje.CheckTrueConditionCount())
            {
                // 將判斷後的權重設在動作權重上
                if (judje.actionWeightAfterJudge != 0)
                    ActionWeight = judje.actionWeightAfterJudge - DiffCount;
                return true;
            }
        }
        return false;
    }

    public void AddDiffCount(float diff)
    {
        DiffCount += diff;
    }

    /// <summary>
    /// 重置權重與權重減值
    /// </summary>
    /// <param name="reduceDiffCount">可決定權重減值要恢復多少，而不是歸0</param>
    public void ResetWeight(float reduceDiffCount = -999)
    {
        ActionWeight = originalActionWeight;
        if (reduceDiffCount == -999)
            DiffCount = 0;
        else
            DiffCount -= reduceDiffCount;
    }

    public abstract bool StartActHaviour();
}

public enum AiActionType 
{
    Idle,
    FackAction, // 不做任何實際行動，類似威嚇、咆嘯、示威...等動作
    Move,
    Retreat,
    Attack,
    SkillAttack,
    Magic,
    Effect,
}

