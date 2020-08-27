using UnityEngine;

public class Teleport : DisposableSkill
{
    public Transform lastAttackTarget;
    public AudioClip teleportSound;
    public float timeSlowDuration;

    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(TeleportToTargetBack);
        immediatelyAffect.AddListener(TimeSlow);
    }

    /// <summary>
    /// Teleport to last attack target back. If target is null, teleport foward.
    /// </summary>
    private void TeleportToTargetBack()
    {
        lastAttackTarget = sourceCaster.combatController.lastAttackTarget;
        // Still can teleport.
        if (lastAttackTarget == null)
        {
            sourceCaster.transform.position = new Vector3(sourceCaster.transform.position.x + sourceCaster.transform.right.x * currentSkill.range.Value, sourceCaster.transform.position.y);
            soundControl.PlaySound(teleportSound);
            return;
        }

        // Need in skill range.
        if (Vector3.Distance(lastAttackTarget.transform.position, sourceCaster.transform.position) > currentSkill.range.Value)
            return;

        float x = (lastAttackTarget.transform.position + lastAttackTarget.transform.right * -1.2f).x;
        sourceCaster.transform.position = new Vector3(x, sourceCaster.transform.position.y); 
        soundControl.PlaySound(teleportSound);
    }

    private void TimeSlow()
    {
        TimeScaleController.Instance.DoSlowMotion(0.05f, timeSlowDuration);
    }
}