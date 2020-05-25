using System.Collections;
using UnityEngine;

public class NaraCircleBurst : DisposableSkill
{
    public NaraCircleBurstData data;

    protected override void AddAffectEvent()
    {
        hitAffect.AddListener(Ignite);
    }

    public override void OnTriggerEnter2D(Collider2D targetCol)
    {
        base.OnTriggerEnter2D(targetCol);

        if (!targetCol.CompareTag(sourceCaster.tag))
        {
            DamageTarget();
            hitAffect.Invoke();
            this.gameObject.SetActive(false);
        }
    }

    public override void UseSkill()
    {
        CameraShakeWhenBurst();
        StartCoroutine(SetActiveAfterSkillDone(0.3f));
        base.UseSkill();
    }

    private void Ignite()
    {
        DebuffControl.Instance.Ignite(target, data.igniteDuration);
    }

    private void CameraShakeWhenBurst()
    {
        CameraShake.Instance.ShakeCamera(1f, 0.5f, 0.1f, true);
    }
}

[System.Serializable]
public struct NaraCircleBurstData
{
    public Transform transform;
    public float delay;
    public float igniteDuration;
}
