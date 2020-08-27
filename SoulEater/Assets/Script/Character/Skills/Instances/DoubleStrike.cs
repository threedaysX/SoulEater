using UnityEngine;

public class DoubleStrike : DisposableSkill
{
    protected override void AddAffectEvent()
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D targetCol)
    {
        base.OnTriggerEnter2D(targetCol);

        if (!targetCol.CompareTag(sourceCaster.tag))
        {
            DamageTarget();
            DamageTarget();
        }
    }
}
