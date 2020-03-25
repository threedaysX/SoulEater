using UnityEngine;
using UnityEngine.Events;

// 複製用:
// [CreateAssetMenu(menuName = "Character/Skill/新的SkillClassName")]
[System.Serializable]
public abstract class Skill : SkillEventBase
{
    public string skillName;
    public string description;
    public float cost;
    public float damage;
    public float castTime;
    public float duartion;
    public float coolDown;

    public Animator animator;
    public UnityEvent effect;

    public void InvokeEffect(Character currentCharacter)
    {
        if (currentCharacter == null)
            return;
        character = currentCharacter;
        effect.Invoke();
    }
}
