using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : DisposableSkill
{
    [SerializeField] private Skill fireBallData;
    [SerializeField] private ParticleSystem chargeEffect;   //集氣特效
    [SerializeField] private int moveSpeed;
    [SerializeField] private int igniteDuration;
    [SerializeField] private GameObject projectile;
    [SerializeField] private ProjectileSetting projectileSetting = new ProjectileSetting { };
    [SerializeField] private ProjectileDirectSetting projectileDirectSetting = new ProjectileDirectSetting { };

    private Vector3 fireBallShootDir;

    private void Start()
    {
        ProjectileDataInitializer dataInitializer = new ProjectileDataInitializer(projectileSetting);
        projectileDirectSetting.initialAngleArray = dataInitializer.GetInitialAngle();
    }

    public override void CastSkill()
    {
        base.CastSkill();

        sourceCaster.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = sourceCaster.transform.position + sourceCaster.transform.up * 0.7f;
        //get fire direction
        if (target == null)
            fireBallShootDir = (sourceCaster.transform.position + new Vector3(fireBallData.range.Value * transform.right.x, 0, 0));
        else
            fireBallShootDir = (target.transform.position - transform.position).normalized;
        chargeEffect.Play();
    }

    public override void UseSkill()
    {
        base.UseSkill(); 
    }

    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(ChargeThenShoot);
        hitAffect.AddListener(Ignite);
    }

    private void ChargeThenShoot()
    {
        StartCoroutine(ChargeThenShootCorutine());
    }

    private IEnumerator ChargeThenShootCorutine()
    {
        if (!chargeEffect.IsAlive(false) && inUsingParticle.IsAlive(false))
            transform.position += fireBallShootDir * moveSpeed * Time.deltaTime;
        else if (!inUsingParticle.IsAlive(false))
            StartCoroutine(ExplodeToProjectile());
        yield break;
    }

    public override void OnTriggerEnter2D(Collider2D targetCol)
    {
        base.OnTriggerEnter2D(targetCol);

        if (targetCol.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            targetCol.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            transform.position = Vector3.zero;
            StartCoroutine(ExplodeToProjectile());
        } 
        else if (targetCol.GetComponent<Character>() != null)
        {
            target = targetCol.GetComponent<Character>();
            DamageTarget();
        }
    }

    private IEnumerator ExplodeToProjectile()
    {
        yield return new WaitForSeconds(1f);
        ProjectileSpawner.Instance.InstantiateProjectile(projectile, projectileSetting, projectileDirectSetting);
        yield break;
    }

    private void Ignite()
    {
        DebuffControl.Instance.Ignite(sourceCaster, target, igniteDuration);
    }
}
