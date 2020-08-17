using UnityEngine;

[System.Serializable]
public struct ProjectileSetting
{
    public Transform initialPosition;
    public int amount;
    [Range(0, 360)]
    public int restrictAngle;
    [Range(0, 360)]
    public int angleOffset;     //從rotation的0度開始加
}

[System.Serializable]
public struct ProjectileDirectSetting
{
    public float moveSpeed;
    public int duration;
    public float freeFlyDuration;
    [HideInInspector]
    public float[] initialAngleArray;
    [HideInInspector]
    public float initialAngle;
    public float angleIncrement;
    public Transform target;
}

public class ProjectileSpawner : Singleton<ProjectileSpawner>
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private int poolSize;

    private void Start()
    {
        ObjectPools.Instance.RenderObjectPoolsInParent(projectile, poolSize);
    }

    public void InstantiateProjectile(GameObject projectilePrefab,  ProjectileSetting projectileSetting, ProjectileDirectSetting projectileDirectSetting)
    {
        int angleChunk = projectileSetting.restrictAngle / projectileSetting.amount;    //在限制角度中平分角度
        int angle = projectileSetting.angleOffset;

        for (int i = 0; i < projectileSetting.amount; i++)
        {
            Transform p = ObjectPools.Instance.GetObjectInPools(projectilePrefab.name, projectileSetting.initialPosition.position).transform;  //object pool
            projectileDirectSetting.initialAngle = projectileDirectSetting.initialAngleArray[i];
            p.GetComponent<Projectile>().ProjectileSetup(projectileDirectSetting);
            
            angle += angleChunk;
        }
    }
}
