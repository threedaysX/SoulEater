using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject p;
    public Transform t;
    public int a;
    public float s;
    public float m;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            InstantiateProjectile(p, transform.position, a, m, 10f, s, t, 1f, 360f);
    }
    
    /// <summary>
    /// 生成投射物
    /// </summary>
    /// <param name="initialPosition">初始位置</param>
    /// <param name="amount">投射物數量</param>
    /// <param name="duration">存在時間</param>
    /// <param name="shootAngleIncrement">軌跡偏離角度</param>
    /// <param name="freeFlyDuration">飛往目標前沒目標自由飛時間</param>
    /// <param name="initialRestrictAngle">限制在多少角度內</param>
    public void InstantiateProjectile(GameObject projectilePrefab, Vector2 initialPosition, int amount, float moveSpeed, float duration,  float shootAngleIncrement, Transform target = null, float freeFlyDuration = 0f, float initialRestrictAngle = 360f)
    {
        float angleChunk = initialRestrictAngle / amount;    //在限制角度中平分角度
        float angle = 0f;

        for (int i = 0; i < amount; i++)
        {
            float dirX = initialPosition.x + Mathf.Sin(angle * Mathf.Deg2Rad);
            float dirY = initialPosition.y + Mathf.Cos(angle * Mathf.Deg2Rad);

            Vector2 projectileVector = new Vector2(dirX, dirY);
            Vector2 projectileDir = (projectileVector - initialPosition).normalized;
            float initialAngle = Mathf.Atan2(projectileDir.y, projectileDir.x);  //Rad
            float radShootAngleIncrement = shootAngleIncrement * Mathf.Deg2Rad / 60;  //Rad

            Transform projectile = Instantiate(projectilePrefab, initialPosition, Quaternion.identity).transform;
            projectile.GetComponent<Projectile>().ProjectileSetup(moveSpeed, duration, initialAngle, radShootAngleIncrement, target, freeFlyDuration);

            angle += angleChunk;
        }
    }
}
