using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicIdle")]
public class BasicIdle : Action
{
    public override bool StartActHaviour()
    {
        return true;
    }
}
