using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicRetreat")]
public class BasicRetreat : Action
{
    public override bool StartActHaviour()
    {
        return Retreat();
    }

    private bool Retreat()
    {
        Vector3 retreatDirection = Vector3.Normalize(-(ai.chaseTarget.position - ai.transform.position));
        ai.transform.position += retreatDirection * ai.data.moveSpeed.Value * Time.deltaTime;
        Debug.Log(ai.characterName + "要撤退摟！！");
        return true;
    }
}
