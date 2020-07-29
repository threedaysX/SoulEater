using System.Collections;
using UnityEngine;

public class TutorialTrapTrigger : MonoBehaviour
{
    [SerializeField] private ProjectileSetting projectileSetting;
    [SerializeField] private ProjectileDirectSetting projectileDirectSetting;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float slowDownFactor;
    [SerializeField] private float slowDownTime;

    private bool canSpawn = true;

    private void Start()
    {
        ProjectileDataInitializer ProjectileData = new ProjectileDataInitializer(projectileSetting);
        projectileDirectSetting.initialAngleArray = ProjectileData.GetInitialAngle();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canSpawn)
        {
            SlowMotionController.Instance.DoSlowMotion(slowDownFactor, slowDownTime);
            ProjectileSpawner.Instance.InstantiateProjectile(projectile, projectileSetting, projectileDirectSetting);
            canSpawn = false;
        }
    }
}
