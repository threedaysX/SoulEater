using System.Collections;
using UnityEngine;

public class TimeScaleController : Singleton<TimeScaleController>
{

    private void Start()
    {
        TimeScale.global = new TimeScaleData();    
    }

    private void Update()
    {
        Time.timeScale = TimeScale.global.currentTimeScale;
    }

    public void OpenUI(bool open)
    {
        if (open)
        {
            TimeScale.global.PauseMotion(true);
            TimeScale.global.originTimeScale = TimeScale.global.currentTimeScale;
            TimeScale.global.currentTimeScale = 0f;
        }
        else
        {
            TimeScale.global.PauseMotion(false);
            TimeScale.global.currentTimeScale = TimeScale.global.originTimeScale;
        }
    }

    public void DoSlowMotion(float slowdownFactor, float slowdownLength)
    {
        StartCoroutine(TimeScale.global.CallMotion(DoSlowMotionCoroutine(slowdownFactor, slowdownLength)));
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
    // Default Timescales
    public static TimeScaleData global;
}

public class TimeScaleData
{
    public float originTimeScale = 1;
    public float currentTimeScale = 1;
    public float slowdownDuration = 1;

    public bool stop;
    public bool running;
    public bool paused;
    public bool finished;

    public IEnumerator CallMotion(IEnumerator action)
    {
        if (running)
        {
            StopMotion();
            yield return new WaitForEndOfFrame();
        }
        if (!running)
        {
            StartMotion();
        }

        IEnumerator e = action;
        while (running)
        {
            if (stop)
                break;

            if (paused)
                yield return null;
            else
            {
                if (e != null && e.MoveNext())
                {
                    yield return e.Current;
                }
                else
                {
                    running = false;
                    break;
                }
            }
        }
        finished = true;
    }

    public void StartMotion()
    {
        stop = false;
        finished = false;
        paused = false;
        running = true;
    }

    public void StopMotion()
    {
        stop = true;
        running = false;
    }

    public void PauseMotion(bool paused)
    {
        this.paused = paused;
    }
}