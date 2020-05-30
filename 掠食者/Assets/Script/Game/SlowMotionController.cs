using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionController : Singleton<SlowMotionController>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 調整整體動畫速度
    public void ModifyAllAnimSpeed(Animator anim, float newSpeed)
    {
        anim.speed = newSpeed;
    }
}

public class TimeScale
{
    //default timescales
    public static TimeScaleData player;
    public static TimeScaleData enemies;
    public static TimeScaleData global;
}

public class TimeScaleData
{
    public float originTimeScale = 1;
    public float currentTimeScale = 1;
}