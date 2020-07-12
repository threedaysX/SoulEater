using System.Collections.Generic;
using UnityEngine;

public class BossModel : EnemyModel
{
    [Header("Boss觸發根性效果")]
    public ParticleSystem knockOutRootParticle;
    public AudioClip knockOutRootSound;
    protected bool startDie = false;

    public override void Start()
    {
        base.Start();
        if (CurrentHealth > 0)
        {
            startDie = false;
        }
    }

    public override void Die()
    {
        if (enemyLevel == EnemyLevel.Boss)
        {
            // Boss would avoid die once.
            if (!startDie)
            {
                operationSoundController.PlaySound(knockOutRootSound);
                knockOutRootParticle.Play(true);
                LockHealthAvoidDie(2f);
            }
            else
            {
                base.Die();
            }
        }
        else
        {
            base.Die();
        }
    }

    protected virtual void LockHealthAvoidDie(float duration)
    {
        CurrentHealth = 1f;
        StopAllCoroutines();
        GetIntoImmune(duration);
        LockOperation(LockType.Die, true);
        operationController.InterruptAnimOperation();
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(FadeScreen.Instance.FadeIn(duration));
        TimeScaleController.Instance.DoSlowMotion(0.05f, duration);

        // Highlight Objs
        List<GameObject> highlightObjs = new List<GameObject>();
        highlightObjs.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        highlightObjs.Add(this.gameObject);
        FadeScreen.Instance.HighlightObjects(0.4f, highlightObjs.ToArray());

        FaceTarget(true);

        startDie = true;
    }
}
