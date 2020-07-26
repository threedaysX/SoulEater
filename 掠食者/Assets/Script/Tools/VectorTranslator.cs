using UnityEngine;

public class VectorTranslator : Singleton<VectorTranslator>
{
    public float GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float x = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (x < 0) x += 360;

        return x;
    }
}
