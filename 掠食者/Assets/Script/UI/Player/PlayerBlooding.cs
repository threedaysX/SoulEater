using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBlooding : MonoBehaviour
{
    public Image blood;
    public Player player;
    public PlayerBloodSetting minorInjuredSetting;
    public PlayerBloodSetting seriousInjuredSetting;
    private Sequence bloodingSeq;
    private bool nonInjured;        // 無傷
    private bool minorInjured;      // 輕傷
    private bool seriousInjured;    // 重傷
    private bool injuredTrigger;

    private void Start()
    {
        injuredTrigger = false;
    }

    private void Update()
    {
        CheckInjured();
        if (injuredTrigger && seriousInjured)
        {
            Blooding(seriousInjuredSetting);
            injuredTrigger = false;
        }
        else if (injuredTrigger && minorInjured)
        {
            Blooding(minorInjuredSetting);
            injuredTrigger = false;
        }
        else if (injuredTrigger && nonInjured)
        {
            StopBleed();
            injuredTrigger = false;
        }
    }

    public void Blooding(PlayerBloodSetting bloodSetting)
    {
        bloodingSeq = DOTween.Sequence();
        bloodingSeq.Append(blood.DOFade(bloodSetting.bloodFade, bloodSetting.fadeDuration))
                   .Append(blood.DOFade(bloodSetting.bloodFadeBreath, bloodSetting.fadeBreathDuration));
        bloodingSeq.SetLoops(int.MaxValue, LoopType.Yoyo);
        bloodingSeq.Play();
    }

    public void StopBleed()
    {
        bloodingSeq.Complete();
        bloodingSeq.Pause();
        bloodingSeq.Kill();
    }

    private void CheckInjured()
    {
        // 當不在重傷狀態時，就會檢查自己是否為重傷
        if (!seriousInjured && player.CurrentHealth <= player.data.maxHealth.Value * 0.2f)
        {
            seriousInjured = true;
            minorInjured = false;
            nonInjured = false;
            injuredTrigger = true;
        }
        // 當不在輕傷狀態時，就會檢查自己是否為輕傷
        else if (!minorInjured && player.CurrentHealth > player.data.maxHealth.Value * 0.2f && player.CurrentHealth <= player.data.maxHealth.Value * 0.5f)
        {
            seriousInjured = false;
            minorInjured = true;
            nonInjured = false;
            injuredTrigger = true;
        }
        else if (!nonInjured && player.CurrentHealth > player.data.maxHealth.Value * 0.5f)
        {
            seriousInjured = false;
            minorInjured = false;
            nonInjured = true;
            injuredTrigger = true;
        }
    }
}

[System.Serializable]
public struct PlayerBloodSetting
{
    // 第一段
    public float bloodFade;
    public float fadeDuration;
    // 第二段 (變成呼吸效果)
    public float bloodFadeBreath;
    public float fadeBreathDuration;
}
