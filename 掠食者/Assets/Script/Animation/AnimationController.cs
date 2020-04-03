using System.Collections;
using UnityEngine;

public class AnimationController : Singleton<AnimationController>
{
    public float GetCurrentAnimationLength(Animator anim)
    {
        return anim.GetCurrentAnimatorStateInfo(0).length;
    }

    public void PlayAnimation(Animator anim, float duration)
    {
        StartCoroutine(PlayAnimInterval(anim, GetCurrentAnimationLength(anim), duration));
    }

    /// <summary>
    /// 反覆播放動畫
    /// </summary>
    /// <param name="duration">總持續時間</param>
    /// <param name="animInterval">動畫撥放一次性時間</param>
    private IEnumerator PlayAnimInterval(Animator anim, float animInterval, float duration)
    {
        if (duration == 0)
        {
            anim.Play("Trigger", -1, 0f);
            yield return new WaitForSeconds(GetCurrentAnimationLength(anim));
        }
        else
        {
            while (duration > 0)
            {
                anim.Play("Trigger", -1, 0f);
                yield return new WaitForSeconds(animInterval);
                duration -= animInterval;
            }
        }

        DestroyAfterAnimationStop(anim);
    }

    public void DestroyAfterAnimationStop(Animator anim)
    {
        Destroy(anim.gameObject);
    }
}
