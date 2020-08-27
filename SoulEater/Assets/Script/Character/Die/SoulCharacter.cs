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
        BurstSoul();
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
        ChnageLayer(burstParticle.gameObject, true, 0);
        burstParticle.Play(true);
    }

    public void BurstSoul()
    {
        //// Old Attractor. Before 2020.08.21.
        //ParticleAttractor at = soulParticle.GetComponent<ParticleAttractor>();
        //at.SetTargetMaster(lastAttackMeTarget.transform);

        soulParticle.Play();

        // Player 吸取靈魂
        if (lastAttackMeTarget.GetComponent<Player>() != null)
        {
            #region Setting
            Player pl = lastAttackMeTarget.GetComponent<Player>();
            float duration = soulParticle.main.startLifetime.constant;
            float offset = 0.5f;
            #endregion

            ChnageLayer(soulParticle.gameObject, true, 1);

            pl.TriggerAttractorBurstEffect(duration);
            CameraShake.Instance.ShakeCamera(4f, 4f, 0.2f, duration, true);
            ZoomInSetting zoomInSetting = new ZoomInSetting { finalZoomSize = 5.6f, duration = duration - offset, startDelay = 0f };
            ZoomInSetting zoomOutSetting = new ZoomInSetting { finalZoomSize = 6f, duration = 0.2f, startDelay = offset };
            CinemachineCameraControl.Instance.ZoomInCamera(zoomInSetting, zoomOutSetting);
        }
    }

    private void ChnageLayer(GameObject gameObject, bool changeChild, int layerOffset)
    {
        string layerName = GetComponent<SpriteRenderer>().sortingLayerName;
        int layerOrder = GetComponent<SpriteRenderer>().sortingOrder;

        gameObject.GetComponent<Renderer>().sortingLayerName = layerName;
        gameObject.GetComponent<Renderer>().sortingOrder = layerOrder + layerOffset;

        if (!changeChild)
            return;

        foreach (Transform item in gameObject.transform)
        {
            item.GetComponent<Renderer>().sortingLayerName = layerName;
            item.GetComponent<Renderer>().sortingOrder = layerOrder + layerOffset;
        }
    }
}