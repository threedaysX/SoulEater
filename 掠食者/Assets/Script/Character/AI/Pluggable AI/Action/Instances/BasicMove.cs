using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicMove")]
public class BasicMove : Action
{
    public override bool StartActHaviour()
    {
        return Move();
    }

    private bool Move()
    {
        Vector3 chaseDirection = Vector3.Normalize(ai.chaseTarget.position - ai.transform.position);
        ai.transform.position += chaseDirection * ai.data.moveSpeed.Value * Time.deltaTime;
        Debug.Log(ai.characterName + "要移動摟！！");
        return true;
    }
}
