using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/PlayerGetCloser")]
public class PlayerGetCloser : JudgeCondition
{
    [Header("玩家接近多少")]
    public float distancePlayerGetClose;
    [Header("在幾秒內")]
    public float timeToGetClose;
    
    public override bool CheckActConditionHaviour()
    {
        if (ai.distanceDetect != null)
        {
            ai.distanceDetect.customDistance = distancePlayerGetClose;
            ai.distanceDetect.timeToAct = timeToGetClose;
            return ai.distanceDetect.hasGetClose;
        }
        else
            return false;
    }
}
