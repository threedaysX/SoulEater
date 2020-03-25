﻿using UnityEngine;

/// <summary>
/// AI行為判斷基礎。
/// (每個)判斷含有單一、複合式條件。
/// </summary>
[CreateAssetMenu(menuName = "Character/AI/Judgement")]
public class Judgement : AIHaviourBase
{
    public int conditionTrueCount = 0;
    public JudgeCondition[] conditions;

    public void StartCheckActCondition()
    {
        foreach (JudgeCondition condition in conditions)
        {
            if (condition.CheckActConditionHaviour())
            {
                conditionTrueCount++;
            }
        }
    }

    public bool CheckTrueConditionCount()
    {
        if (conditionTrueCount == conditions.Length)
        {
            return true;
        }
        return false;
    }
}