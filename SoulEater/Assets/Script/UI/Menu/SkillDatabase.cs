using UnityEngine;
using System.Linq;

public class SkillDatabase : MonoBehaviour
{
    public Character player;
    public Transform skillInventory;
    public SkillSlot[] skillSlots;
    public AttackType dbSkillType;

    private void Start()
    {
        skillSlots = skillInventory.GetComponentsInChildren<SkillSlot>();
        ResetSkillInventory();
    }

    public void ResetSkillInventory()
    {
        Skill[] skills = player.skillFields.Where(x => x.skillType == this.dbSkillType).ToArray();
        int slotIndex = 0;
        foreach (var skill in skills)
        {
            skillSlots[slotIndex++].AddSkill(skill.icon, skill);
        }
    }
}
