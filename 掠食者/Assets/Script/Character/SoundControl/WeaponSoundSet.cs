using UnityEngine;

[CreateAssetMenu(menuName = "WeaponSounds")]
public class WeaponSoundSet : ScriptableObject
{
    public SoundSet hitSound = new SoundSet(SoundType.Hit);
    public SoundSet attackSound = new SoundSet(SoundType.Attack);
    public SoundSet castSound = new SoundSet(SoundType.Cast);
    public SoundSet useSkillSound = new SoundSet(SoundType.UseSkill);
}
