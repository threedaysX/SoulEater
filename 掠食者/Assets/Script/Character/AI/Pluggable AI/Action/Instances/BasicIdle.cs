using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicIdle")]
public class BasicIdle : AiAction
{
    public override bool StartActHaviour()
    {
        // 重置移動動畫
        ai.operationController.StartMoveAnim(0);
        return true;
    }
}
