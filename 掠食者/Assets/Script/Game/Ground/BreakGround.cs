using System.Collections;
using UnityEngine;

public class BreakGround : MonoBehaviour
{
    public string breakGroundNameWithAnimPlay = "BreakGround";
    public float rebornTimes;
    public GroundType groundType;
    public Animator anim;

    private bool startBreakTrigger;
    private bool startBreak;

    private void Start()
    {
        startBreakTrigger = true;
        startBreak = false;
    }

    private void Update()
    {
        if (startBreak && startBreakTrigger)
        {
            StartCoroutine(BreakObjInTimes(anim, groundType, rebornTimes));
            startBreakTrigger = false;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        // Enabled that check col is really stay / stand on this ground. (with platform effector 2D)
        if (!startBreak && col.enabled)
        {
            startBreak = true;
            startBreakTrigger = true;
        }
    }

    public IEnumerator BreakObjInTimes(Animator anim, GroundType groundType, float rebornTimes = 0)
    {
        startBreak = false;
        bool needReborn = false;
        switch (groundType)
        {
            case GroundType.Basic:
                yield break;
            case GroundType.BreakAndReborn:
                needReborn = true;
                break;
            case GroundType.BreakNotReborn:
                break;
            case GroundType.Summon:
                break;
            default:
                break;
        }
        // play break anim, then destory.
        anim.Play(breakGroundNameWithAnimPlay);
        yield return new WaitForSeconds(Time.deltaTime);
        yield return new WaitForSeconds(AnimationBase.Instance.GetCurrentAnimationLength(anim));
        
        if (needReborn)
        {
            anim.gameObject.GetComponent<Collider2D>().enabled = false;
            anim.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(rebornTimes);
            anim.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            anim.gameObject.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            Destroy(anim.gameObject);
        }
    }
}

public enum GroundType
{
    Basic,  // 一般平台
    BreakAndReborn, // 踩上平台會碎裂，後會自動重生
    BreakNotReborn, // 踩上平台會碎裂，不會重生
    Summon  // 被召喚出來的平台，踩上平台會碎裂，不會重生
}