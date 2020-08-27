using System;
using System.Collections;
using UnityEngine;

public class Counter : Singleton<Counter>
{
    /// <summary>
    /// CountDown and get coroutine data, and set float with setter.
    /// </summary>
    public Coroutine CountDown(float timer, Action<float> setter)
    {
        return StartCoroutine(StartCountDown(timer, setter));
    }

    /// <summary>
    /// CountDown a number within duration, and set float with setter.
    /// And get coroutine data.
    /// </summary>
    public Coroutine CountDownInTimes(float originNumber, float endNumber, float duration, Action<float> setter, params Action[] callback)
    {
        return StartCoroutine(StartCountInTimes(originNumber, endNumber, duration, setter, callback));
    }

    /// <summary>
    /// CountDown a number within duration, and callback action at the end.
    /// And get coroutine data.
    /// </summary>
    public Coroutine CountDownInTimes(float originNumber, float endNumber, float duration, params Action[] callback)
    {
        return StartCoroutine(StartCountInTimes(originNumber, endNumber, duration, callback));
    }

    #region IEnumerator
    private IEnumerator StartCountDown(float timer, Action<float> setter)
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            setter.Invoke(timer);
            yield return timer;
        }
    }

    private IEnumerator StartCountInTimes(float originNumber, float endNumber, float duration, Action<float> setter, params Action[] callback)
    {
        float timeleft = duration;
        float step = originNumber - endNumber;
        float resultCount = originNumber;
        while (timeleft > 0)
        {
            if (timeleft > Time.deltaTime)
                resultCount -= (step * Time.deltaTime / duration);
            else
                resultCount -= (step * timeleft / duration);

            timeleft -= Time.deltaTime;
            setter.Invoke(resultCount);
            yield return resultCount;
        }

        foreach (Action item in callback)
        {
            item.Invoke();
        }
    }

    private IEnumerator StartCountInTimes(float originNumber, float endNumber, float duration, params Action[] callback)
    {
        float timeleft = duration;
        float step = originNumber - endNumber;
        float resultCount = originNumber;
        while (timeleft > 0)
        {
            if (timeleft > Time.deltaTime)
                resultCount -= (step * Time.deltaTime / duration);
            else
                resultCount -= (step * timeleft / duration);

            timeleft -= Time.deltaTime;
            yield return resultCount;
        }

        foreach (Action item in callback)
        {
            item.Invoke();
        }
    }
    #endregion
}
