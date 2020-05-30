using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionController : Singleton<SlowMotionController>
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    public bool isOpenUI;

    private void Start()
    {
        isOpenUI = false;
    }

    private void Update()
    {
        if (!isOpenUI)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
    }

    public void DoSlowMotion(float slowdownFactor, float slowdownLength)
    {
        this.slowdownLength = slowdownLength;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    // 調整整體動畫速度
    public void ModifyAllAnimSpeed(Animator anim, float newSpeed)
    {
        anim.speed = newSpeed;
    }
}

public enum SlowMotionTargetType
{
    Player,
    Enemy,
    Global
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
    public float slowDownDuration = 1;
}