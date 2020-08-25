using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Character sourceCaster;
    private ElementType elementType;
    private float moveSpeed;
    private int duration;
    private float initialAngle;
    private float shootAngleIncrement;
    private Transform target;
    private float freeFlyDuration;

    private float timer;
    private Vector2 finalDirection;

    public void ProjectileSetup(ProjectileDirectSetting projectileDirectSetting)
    {
        if(projectileDirectSetting.sourceCaster != null)
        {
            this.sourceCaster = projectileDirectSetting.sourceCaster;
            this.elementType = sourceCaster.data.attackElement;
        }
        this.moveSpeed = projectileDirectSetting.moveSpeed;
        this.duration = projectileDirectSetting.duration;
        this.initialAngle = projectileDirectSetting.initialAngle;  //rad
        this.shootAngleIncrement = projectileDirectSetting.angleIncrement;   //deg
        this.target = projectileDirectSetting.target;
        this.freeFlyDuration = projectileDirectSetting.freeFlyDuration;
    }

    private void LateUpdate()
    {
        if(gameObject.activeInHierarchy && timer <= duration)
        {
            timer += Time.deltaTime;
            ProjectilePattern();
        }
    }

    private void ProjectilePattern()
    {
        transform.eulerAngles = new Vector3(0 , 0, initialAngle);

        initialAngle += (shootAngleIncrement * Mathf.Deg2Rad * Time.deltaTime);
        finalDirection = new Vector2(Mathf.Cos(initialAngle), Mathf.Sin(initialAngle));

        if (target != null)
        {
            if(timer < freeFlyDuration)
                transform.position += (Vector3)finalDirection * moveSpeed * Time.deltaTime;
            else if(timer < freeFlyDuration + 0.1f) { }
            else
                transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
            transform.position += (Vector3)finalDirection * moveSpeed * Time.deltaTime;

        if(timer > duration)
        {
            gameObject.SetActive(false);
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character targetToDamage = collision.GetComponent<Character>();

        if (targetToDamage  != null && targetToDamage != sourceCaster && sourceCaster != null)
        {
            float damage = DamageController.Instance.GetAttackDamage(sourceCaster, targetToDamage, AttackType.Attack, elementType, out bool isCritical);
            collision.GetComponent<Character>().TakeDamage(sourceCaster.gameObject, (int)damage, isCritical);
        }
        else if(targetToDamage == sourceCaster && sourceCaster == null)
        {
            return;
        }
        gameObject.SetActive(false);
    }
}
