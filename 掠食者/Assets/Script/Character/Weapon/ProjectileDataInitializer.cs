using UnityEngine;

public class ProjectileDataInitializer
{
    public ProjectileDirectSetting ProjectileDirectSetting;

    private Vector2 initialPosition;
    private int amount;
    private int restrictAngle;
    private int angleOffset;

    public ProjectileDataInitializer(ProjectileSetting projectileSetting)
    {
        this.initialPosition = projectileSetting.initialPosition.position;
        this.amount = projectileSetting.amount;
        this.restrictAngle = projectileSetting.restrictAngle;
        this.angleOffset = projectileSetting.angleOffset;
    }

    public float[] GetInitialAngle()    //rad
    {
        int angleChunk = restrictAngle / amount;    //在限制角度中平分角度
        int angle = angleOffset;
        float[] initialAngle = new float[amount];
        
        for (int i = 0; i < amount; i++)
        {
            float dirX = initialPosition.x + Mathf.Sin(angle * Mathf.Deg2Rad);
            float dirY = initialPosition.y + Mathf.Cos(angle * Mathf.Deg2Rad);

            Vector2 projectileVector = new Vector2(dirX, dirY);
            Vector2 projectileDir = (projectileVector - initialPosition).normalized;
            initialAngle[i] = Mathf.Atan2(projectileDir.y, projectileDir.x);
            
            angle += angleChunk;
        }
        
        return initialAngle;
    }
}
