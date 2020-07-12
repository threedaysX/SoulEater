using System.Collections;
using UnityEngine;

public class NaraCircleBurst : DisposableSkill
{
    public NaraCircleBurstData data;
    public bool isBursted;

    protected override void AddAffectEvent()
    {
        hitAffect.AddListener(Ignite);
    }

    public override void OnTriggerEnter2D(Collider2D targetCol)
    {
        base.OnTriggerEnter2D(targetCol);

        if (sourceCaster != null && !targetCol.CompareTag(sourceCaster.tag))
        {
            DamageTarget();
            hitAffect.Invoke();
            this.gameObject.SetActive(false);
        }
    }

    public override void GenerateSkill(Character character, Skill skill)
    {
        StartCoroutine(CheckExistState());
        base.GenerateSkill(character, skill);
    }

    public override void UseSkill()
    {
        isBursted = true;
        CameraShakeWhenBurst();
        StartCoroutine(SetActiveAfterSkillDone(0.3f));
        base.UseSkill();
    }

    private void Ignite()
    {
        DebuffControl.Instance.Ignite(sourceCaster, target, data.igniteDuration);
    }

    private void CameraShakeWhenBurst()
    {
        CameraShake.Instance.ShakeCamera(1f, 0.5f, 0.1f, 0f, true);
    }

    /// <summary>
    /// Check object need to set active false or not (Exists too long but not burst).
    /// </summary>
    private IEnumerator CheckExistState()
    {
        yield return new WaitForSeconds(data.delay + 6f);
        if (!isBursted)
        {
            SetActiveAfterSkillDone(false);
        }
    }
}

[System.Serializable]
public struct NaraCircleBurstData
{
    public Transform transform;
    public float delay;
    public float igniteDuration;
}
