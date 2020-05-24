using UnityEngine;

public class OperationSoundController : MonoBehaviour
{
    [Header("基本音效")]
    public SoundSet idleSound = new SoundSet(SoundType.Idle);
    public SoundSet moveSound = new SoundSet(SoundType.Move);
    public SoundSet jumpSound = new SoundSet(SoundType.Jump);
    public SoundSet groundTouchSound = new SoundSet(SoundType.GroundTouch);
    public SoundSet evadeSound = new SoundSet(SoundType.Evade);
    public SoundSet collectionSound = new SoundSet(SoundType.Collect);

    [Header("武器音效")]
    public SoundSet hitSound = new SoundSet(SoundType.Hit);
    public SoundSet attackSound = new SoundSet(SoundType.Attack);
    public SoundSet castSound = new SoundSet(SoundType.Cast);
    public SoundSet useSkillSound = new SoundSet(SoundType.UseSkill);

    [Header("音效物件")]
    public AudioSource masterSoundAudio;
    public AudioSource effectSoundAudio;

    private Camera soundPoint; // 根據攝影機位置調整音量大小
    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
        soundPoint = Camera.main;
        ResetCharacterWeaponSoundData(character.data.weaponSoundSet);
    }

    public void PlaySound(SoundSet soundSet)
    {
        if (soundSet.audioClip == null || soundSet.audioClip.Length == 0)
            return;

        effectSoundAudio.clip = soundSet.audioClip[Random.Range(0, soundSet.audioClip.Length)];  // 隨機撥放
        if (effectSoundAudio.clip == null)
            return;
        effectSoundAudio.PlayOneShot(effectSoundAudio.clip);
    }

    public void PlaySound(AudioClip clip, bool adjustSoundByRange = true)
    {
        if (clip == null)
            return;

        if (adjustSoundByRange)
            CalculateSoundDistance();

        effectSoundAudio.PlayOneShot(clip);
    }

    private void CalculateSoundDistance()
    {
        float distanceX = Mathf.Abs(soundPoint.transform.position.x - effectSoundAudio.gameObject.transform.position.x);
        float distanceY = Mathf.Abs(soundPoint.transform.position.y - effectSoundAudio.gameObject.transform.position.y);
        float benchMarkX = 16;  // 左右16m內(3200px)，可以聽到聲音
        float benchMarkY = 12;  // 上下12m內(2400px)，可以聽到聲音
        if (distanceX >= benchMarkX || distanceY >= benchMarkY)
        {
            effectSoundAudio.volume = 0;
        }
        else if (distanceX < benchMarkX && distanceY < benchMarkY)
        {
            float ratioDistance = Mathf.Sqrt(Mathf.Pow(benchMarkX - distanceX, 2) + Mathf.Pow(benchMarkY - distanceY, 2));
            float ratioBenchMark = Mathf.Sqrt(Mathf.Pow(benchMarkX, 2) + Mathf.Pow(benchMarkY, 2));
            effectSoundAudio.volume = ratioDistance / ratioBenchMark;
        }
    }
    
    public void StopSound()
    {
        effectSoundAudio.Stop();
    }

    public void ResetCharacterWeaponSoundData(WeaponSoundSet weaponSoundSet = null)
    {
        if (weaponSoundSet != null)
        {
            hitSound = weaponSoundSet.hitSound;
            attackSound = weaponSoundSet.attackSound;
            castSound = weaponSoundSet.castSound;
            useSkillSound = weaponSoundSet.useSkillSound;
        }
    }
}

[System.Serializable]
public class SoundSet
{
    public SoundType soundType;
    public AudioClip[] audioClip;

    public SoundSet(SoundType soundType)
    {
        this.soundType = soundType;
    }
}

public enum SoundType
{
    Idle,
    Move,
    Jump,
    GroundTouch,
    Evade,
    Collect,        // 收割靈魂音效
    Cast,           // 詠唱中音效
    Attack,         // 攻擊音效
    UseSkill,       // 使用技能時音效
    Hit             // 命中音效
}
