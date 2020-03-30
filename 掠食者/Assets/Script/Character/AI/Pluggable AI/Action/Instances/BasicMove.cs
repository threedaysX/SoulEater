using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicMove")]
public class BasicMove : Action
{
    public override void StartActHaviour()
    {
        Move();
    }

    private void Move()
    {
        Vector3 chaseDirection = ai.chaseTarget.position - ai.transform.position;
        ai.transform.position += chaseDirection * ai.data.moveSpeed.Value * Time.deltaTime;
        Debug.Log(ai.characterName + "要移動摟！！");
    }
}
