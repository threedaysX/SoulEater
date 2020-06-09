using System.Collections;
using UnityEngine;

public class TimeScaleController : Singleton<TimeScaleController>
{
    private void Start()
    {
        TimeScale.global = new TimeScaleData();    
        TimeScale.player = new TimeScaleData();    
        TimeScale.enemies = new TimeScaleData();
    }

    private void Update()
    {
        Time.timeScale = TimeScale.global.currentTimeScale;
    }

    public void OpenUI(bool open)
    {
        if (open)
        {
            TimeScale.global.currentTimeScale = 0f;
        }
        else
        {
            TimeScale.global.currentTimeScale = TimeScale.global.originTimeScale;
        }
    }

    public void DoSlowMotion(float slowdownFactor, float slowdownLength)
    {
        StartCoroutine(DoSlowMotionCoroutine(slowdownFactor, slowdownLength));
    }

    private IEnumerator DoSlowMotionCoroutine(float slowdownFactor, float slowdownLength)
    {
        TimeScale.global.currentTimeScale = slowdownFactor;
        Time.fixedDeltaTime = TimeScale.global.currentTimeScale * 0.02f;
        float timeleft = slowdownLength;
        while (timeleft > 0)
        {
            TimeScale.global.currentTimeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            TimeScale.global.currentTimeScale = Mathf.Clamp(TimeScale.global.currentTimeScale, 0f, 1f);
            timeleft -= Time.deltaTime;
            yield return null;
        }
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
    public float slowdownDuration = 1;
}