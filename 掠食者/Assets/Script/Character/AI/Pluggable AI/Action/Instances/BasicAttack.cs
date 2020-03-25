using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicAttack")]
public class BasicAttack : Action
{
    public override void StartActHaviour()
    {
        Attack();
    }

    private void Attack()
    {
        Debug.Log(ai.characterName + "要攻擊你摟><");
    }
}
