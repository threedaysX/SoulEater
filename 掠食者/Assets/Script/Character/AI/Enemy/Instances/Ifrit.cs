using UnityEngine;

public class Ifrit : Enemy
{
    // 伊夫利特 
    // 150%強度
    public override void Start()
    {
        base.Start();

        SetEnemyLevel(EnemyLevel.Boss);
        GetUnlimitedMana();
    }
}
