using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicMove")]
public class BasicMove : Action
{
    public override void StartActHaviour()
    {
        Test();
    }

    private void Test()
    {
        ai.transform.position += new Vector3(ai.data.moveSpeed.Value * Time.deltaTime, 0, 0);
    }
}
