using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    public Character player;
    public Transform skillInventory;
    public SkillSlot[] skillSlots;

    private void Start()
    {
        skillSlots = skillInventory.GetComponentsInChildren<SkillSlot>();
        ResetSkillInventory();
    }

    public void ResetSkillInventory()
    {
        Skill[] skills = player.skillFields.ToArray();
        int slotIndex = 0;
        foreach (var skill in skills)
        {
            skillSlots[slotIndex++].AddSkill(skill.icon, skill);
        }
    }
}
