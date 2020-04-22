<<<<<<< HEAD
﻿using System.Collections;
using UnityEngine;

public class Player : Character
{

=======
﻿using UnityEngine;

public class Player : Character
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(HotKeyController.attackKey1) || Input.GetKeyDown(HotKeyController.attackKey2))
        {
            operationController.StartAttack(AttackType.Attack, data.attackElement);
        }
    }
>>>>>>> 三天-公式&操作&機制修正
}
