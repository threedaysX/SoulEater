using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : DisposableSkill
{
    public Transform lastAttackTarget;
    public AudioClip teleportSound;

    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(TeleportToTargetBack);
    }

    /// <summary>
    /// Teleport to last attack target back.
    /// </summary>
    private void TeleportToTargetBack()
    {
        lastAttackTarget = sourceCaster.combatController.lastAttackTarget;
        if (lastAttackTarget == null)
            return;
        // Need in skill range.
        if (Vector3.Distance(lastAttackTarget.transform.position, sourceCaster.transform.position) > currentSkill.range)
            return;

        soundControl.PlaySound(teleportSound, true);
        float x = (lastAttackTarget.transform.position + lastAttackTarget.transform.right * -1.2f).x;
        sourceCaster.transform.position = new Vector3(x, sourceCaster.transform.position.y); 
    }
}
