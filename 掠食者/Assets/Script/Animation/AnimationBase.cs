using System;
using System.Collections;
using UnityEngine;

public class AnimationBase : Singleton<AnimationBase>
{
    public float GetAnimationLengthByName(Animator anim, string animationName)
    {
        if (anim == null || anim.runtimeAnimatorController == null)
            return 0;

        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;   
            }
        }
        return 0;
    }

    public float GetAnimationLengthByNames(Animator anim, params string[] animationNameList)
    {
        float resultLength = 0;
        if (anim == null || anim.runtimeAnimatorController == null)
            return resultLength;

        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (string animName in animationNameList)
        {
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == animName)
                {
                    resultLength += clip.length;
                    break;
                }
            }
        }
        return resultLength;
    }

    public float GetCurrentAnimationLength(Animator anim)
    {
        if (anim == null)
            return 0;
        return anim.GetCurrentAnimatorStateInfo(0).length;
    }

    /// <summary>
    /// 因應動畫播放的延遲，需要等待動畫播放後才取得當前播放動畫的時長
    /// </summary>
    /// <param name="getResultLength">取得時間的方法(用來接收結果)</param>
    /// <param name="delayGetDuration">延遲多久才取得動畫時間</param>
    /// <returns></returns>
    public IEnumerator GetCurrentAnimationLength(Animator anim, Func<float, float> getResultLength)
    {
        yield return new WaitForEndOfFrame();
        getResultLength.Invoke(anim.GetCurrentAnimatorStateInfo(0).length);
    }

    public void PlayAnimationLoop(Animator anim, string animationName, float duration, bool destroyAfterAnimStop, bool setActiveAfterAnimStop)
    {
        StartCoroutine(PlayAnimInterval(anim, animationName, GetAnimationLengthByName(anim, animationName), duration, destroyAfterAnimStop, setActiveAfterAnimStop));
    }

    public void PlayAnimationLoop(Animator anim, string animationName, float duration, Action callBack)
    {
        StartCoroutine(PlayAnimInterval(anim, animationName, GetAnimationLengthByName(anim, animationName), duration, callBack));
    }

    /// <summary>
    /// 反覆播放動畫
    /// </summary>
    /// <param name="duration">總持續時間</param>
    /// <param name="animInterval">動畫撥放一次性時間</param>
    private IEnumerator PlayAnimInterval(Animator anim, string animationName, float animInterval, float duration, bool destroyAfterAnimStop, bool setActiveAfterAnimStop)
    {
        if (anim == null)
            yield break;

        if (duration == 0)
        {
            anim.Play(animationName, -1, 0f);
            yield return new WaitForSeconds(animInterval);
        }
        else
        {
            while (duration > 0)
            {
                anim.Play(animationName, -1, 0f);
                yield return new WaitForSeconds(animInterval);
                duration -= animInterval;
            }
        }

        if (destroyAfterAnimStop)
            DestroyAfterAnimationStop(anim);
        else
            SetActiveAfterAnimationStop(anim, setActiveAfterAnimStop);
    }

    /// <summary>
    /// 反覆播放動畫，動畫結束後執行CallBack動作
    /// </summary>
    /// <param name="duration">總持續時間</param>
    /// <param name="animInterval">動畫撥放一次性時間</param>
    /// <param name="callBack">動畫結束後啟用的動作</param>
    private IEnumerator PlayAnimInterval(Animator anim, string animationName, float animInterval, float duration, Action callBack)
    {
        if (anim == null)
            yield break;

        if (duration == 0)
        {
            anim.Play(animationName, -1, 0f);
            yield return new WaitForSeconds(animInterval);
        }
        else
        {
            while (duration > 0)
            {
                if (anim == null)
                    yield break;
                anim.Play(animationName, -1, 0f);
                yield return new WaitForSeconds(animInterval);
                duration -= animInterval;
            }
        }

        callBack.Invoke();
    }

    private void DestroyAfterAnimationStop(Animator anim)
    {
        Destroy(anim.gameObject);
    }

    private void SetActiveAfterAnimationStop(Animator anim, bool setActiveAfterAnimStop)
    {
        anim.gameObject.SetActive(setActiveAfterAnimStop);
    }
}
