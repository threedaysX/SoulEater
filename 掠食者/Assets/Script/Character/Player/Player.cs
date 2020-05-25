using UnityEngine;

public class Player : Character
{

    private void Update()
    {
        if (Input.GetKeyDown(HotKeyController.attackKey1) || Input.GetKeyDown(HotKeyController.attackKey2))
        {
            operationController.StartAttack(AttackType.Attack, data.attackElement);
        }
    }
}
