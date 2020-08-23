using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : DisposableSkill
{
    public ParticleSystem chargeEffect;   //集氣特效
    public int moveSpeed;
    public int igniteDuration;
    public GameObject projectile;
    public ProjectileSetting projectileSetting = new ProjectileSetting { };
    public ProjectileDirectSetting projectileDirectSetting = new ProjectileDirectSetting { };

    private Vector3 fireBallDestination;
    private bool canShootProjectile = false;

    private void Start()
    {
        ProjectileDataInitializer dataInitializer = new ProjectileDataInitializer(projectileSetting);
        projectileDirectSetting.initialAngleArray = dataInitializer.GetInitialAngle();
        projectileDirectSetting.sourceCaster = sourceCaster;
    }

    public override void GenerateSkill(Character character, Skill skill)
    {
        base.GenerateSkill(character, skill);

        canShootProjectile = true;
        transform.position = sourceCaster.transform.position + sourceCaster.transform.up * 0.7f;

        //get fire direction
        target = Physics2D.OverlapCircle(transform.position, skill.range.Value).gameObject.GetComponent<Character>();
        fireBallDestination = (target == null) ? transform.position + transform.right * currentSkill.range.Value : target.transform.position;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if(transform.position.x > fireBallDestination.x - 0.2f && transform.position.x < fireBallDestination.x + 0.2f &&
                transform.position.y > fireBallDestination.y - 0.2f && transform.position.y < fireBallDestination.y + 0.2f)
            {
                //transform.position = Vector2.MoveTowards(transform.position, fireBallDestination + transform.right * 1.5f, moveSpeed / 2 * Time.deltaTime);
                transform.position += transform.right * moveSpeed * 2 * Time.deltaTime;
                if (canShootProjectile)
                {
                    StartCoroutine(ExplodeToProjectile());
                    canShootProjectile = false;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, fireBallDestination, moveSpeed * Time.deltaTime);
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D targetCol)
    {
        base.OnTriggerEnter2D(targetCol);

        if(targetCol.gameObject.GetComponent<Character>() != null && targetCol.gameObject.GetComponent<Character>() != sourceCaster)
        {
            DamageTarget();
            SetActiveAfterSkillDone(false);
        }
        else if(targetCol.CompareTag("EnvironmentCollider"))  //not hit
        {
            Debug.Log("Hit");
            StartCoroutine(ExplodeToProjectile());
        }
    }

    private IEnumerator ExplodeToProjectile()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(SetActiveAfterSkillDone(0.5f));
        ProjectileSpawner.Instance.InstantiateProjectile(projectile, projectileSetting, projectileDirectSetting);
        yield break;
    }
    protected override void AddAffectEvent()
    {
        hitAffect.AddListener(Ignite);
    }

    private void Ignite()
    {
        Debuff.Instance.Ignite(sourceCaster, target, igniteDuration);
    }
}
