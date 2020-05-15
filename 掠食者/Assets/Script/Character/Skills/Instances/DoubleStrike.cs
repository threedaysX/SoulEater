using UnityEngine;

public class DoubleStrike : DisposableSkill
{
    protected override void AddAffectEvent()
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D target)
    {
        base.OnTriggerEnter2D(target);

        if (!target.CompareTag(sourceCaster.tag))
        {
            DamageTarget();
            DamageTarget();
        }
    }
}
