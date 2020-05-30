using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/CheckLastAction")]
public class CheckLastAction : JudgeCondition
{
    public AiAction lastAction;
    [Header("非上一個動作")]
    public bool isFalse;
    public override bool CheckActConditionHaviour()
    {
        return LastAction();
    }

    //抓取可在Inspector自訂的Action
    private bool LastAction()
    {
        bool isTargetAction = false;
        if(ai.lastAction != null)
        {
            if (ai.lastAction == lastAction) 
            {
                isTargetAction = true;
            }
        }

        if (isFalse)
        {
            isTargetAction = !isTargetAction;
        }

        return isTargetAction;
    }
}
