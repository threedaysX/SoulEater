using UnityEngine;

public class ShockWave : DisposableSkill
{
    public ParticleSystem castHintEffect;
    [Header("衝擊擊退力道")]
    public float waveKnockBackForce;

    public override void OnTriggerEnter2D(Collider2D targetCol)
    {
        base.OnTriggerEnter2D(targetCol);

        if (!targetCol.CompareTag(sourceCaster.tag))
        {
            DamageTarget();
            InvokeAffect(hitAffect);
        }
    }

    protected override void AddAffectEvent()
    {
        hitAffect.AddListener(DebuffSlowDown);
        hitAffect.AddListener(DebuffTired);
        hitAffect.AddListener(KnockBackHitTarget);
    }

    public override void CastSkill()
    {
        base.CastSkill();
        ShiningCastHintEffectOnBody();
    }

    /// <summary>
    /// 對命中的敵人造成-50%移動速度，持續1秒
    /// </summary>
    private void DebuffSlowDown()
    {
        Debuff.Instance.SlowMoveSpeed(target, 50, 1f);
    }

    public string tired = "疲累";
    /// <summary>
    /// 被命中的敵人無法使用跳躍，持續0.6秒
    /// </summary>
    private void DebuffTired()
    {
        Debuff.Instance.Lame(target, 0.6f);
    }

    private void KnockBackHitTarget()
    {
        KnockStunSystem targetKnock = target.GetComponent<KnockStunSystem>();
        float directionX = new Vector2(sourceCaster.transform.position.x - target.transform.position.x, 0).normalized.x;
        targetKnock.KnockStun(target, directionX, waveKnockBackForce);
    }

    private void ShiningCastHintEffectOnBody()
    {
        var bodyPosY = sourceCaster.transform.position.y + sourceCaster.GetComponent<SpriteRenderer>().bounds.size.y / 2 - 1f;
        castHintEffect.transform.position = new Vector2(sourceCaster.transform.position.x, bodyPosY);
        castHintEffect.Play(true);
    }
}