using System;
using System.Collections;
using UnityEngine;

public class AnimationBase : Singleton<AnimationBase>
{
    public float GetCurrentAnimationLength(Animator anim, string animationName)
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
    public IEnumerator GetCurrentAnimationLength(Animator anim, Func<float, float> getResultLength, float delayGetDuration = 0.05f)
    {
        yield return new WaitForSeconds(delayGetDuration);
        getResultLength.Invoke(anim.GetCurrentAnimatorStateInfo(0).length);
    }

    public void PlayAnimationLoop(Animator anim, string animationName, float duration, bool destroyAfterAnimStop, bool setActiveAfterAnimStop)
    {
        StartCoroutine(PlayAnimInterval(anim, animationName, GetCurrentAnimationLength(anim, animationName), duration, destroyAfterAnimStop, setActiveAfterAnimStop));
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

    private void DestroyAfterAnimationStop(Animator anim)
    {
        Destroy(anim.gameObject);
    }

    private void SetActiveAfterAnimationStop(Animator anim, bool setActiveAfterAnimStop)
    {
        anim.gameObject.SetActive(setActiveAfterAnimStop);
    }
}
