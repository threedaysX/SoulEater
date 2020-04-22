using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/CheckLastActionType")]
public class CheckLastActionType : JudgeCondition
{
    public Action lastActionToCheckType;
    [Header("非上一個動作")]
    public bool isFalse;
    public override bool CheckActConditionHaviour()
    {
        return LastActionType();
    }

    private bool LastActionType()
    {
        bool isTargetActionType = false;
        if(ai.lastAction != null)
        {
            if (ai.lastAction.actionType == lastActionToCheckType.actionType) //lastAction null!!!
            {
                isTargetActionType = true;
            }
        }

        if (isFalse)
        {
            isTargetActionType = !isTargetActionType;
        }

        return isTargetActionType;
    }
}
