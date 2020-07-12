using System.Collections;
using UnityEngine;

public class SoulCharacter : Character
{
    [Header("Die Setting")]
    public ParticleSystem burstParticle;
    public ParticleSystem soulParticle;
    public float soulDieDuration;

    public override void Die()
    {
        // Give Frag.
        TimeScaleController.Instance.DoSlowMotion(0.05f, 3f);
        TriggerDieEffect();
        AbsorbSoul();
        StartCoroutine(DelayDestory(soulDieDuration));
    }

    private IEnumerator DelayDestory(float delay)
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
        EndGameManager.Instance.EndGame();
    }

    private void TriggerDieEffect()
    {
        ChnageLayer(burstParticle.gameObject, true);
        burstParticle.Play(true);
    }

    public void AbsorbSoul()
    {
        ParticleAttractor at = soulParticle.GetComponent<ParticleAttractor>();
        at.SetTargetMaster(lastAttackMeTarget.transform);
        ChnageLayer(soulParticle.gameObject, true);
        soulParticle.Play();

        CameraShake.Instance.ShakeCamera(4f, 4f, 0.2f, 5f, true);
        ZoomInSetting zoomInSetting = new ZoomInSetting { finalZoomSize = 5.6f, duration = 4.5f, startDelay = 0f };
        ZoomInSetting zoomOutSetting = new ZoomInSetting { finalZoomSize = 6f, duration = 0.2f, startDelay = 0.5f };
        CinemachineCameraControl.Instance.ZoomInCamera(zoomInSetting, zoomOutSetting);
    }

    private void ChnageLayer(GameObject gameObject, bool changeChild)
    {
        string layerName = GetComponent<SpriteRenderer>().sortingLayerName;
        int layerOrder = GetComponent<SpriteRenderer>().sortingOrder;

        gameObject.GetComponent<Renderer>().sortingLayerName = layerName;
        gameObject.GetComponent<Renderer>().sortingOrder = layerOrder;

        if (!changeChild)
            return;

        foreach (Transform item in gameObject.transform)
        {
            item.GetComponent<Renderer>().sortingLayerName = layerName;
            item.GetComponent<Renderer>().sortingOrder = layerOrder;
        }
    }
}