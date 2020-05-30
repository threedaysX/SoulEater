using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicMove")]
public class BasicMove : AiAction
{
    public float basicMoveSpeed = 2.5f;
    public override bool StartActHaviour()
    {
        return Move();
    }

    private bool Move()
    {
        Vector3 chaseDirection = Vector3.Normalize(ai.chaseTarget.position - ai.transform.position);
        chaseDirection.y = 0f;
        chaseDirection.z = 0f;
        ai.transform.position += chaseDirection * basicMoveSpeed * ai.data.moveSpeed.Value * Time.deltaTime;
        ai.operationController.StartMoveAnim(ai.transform.right.x);
        return true;
    }
}
