using UnityEngine;
using UnityEngine.UI;

public class SkillDescription : Singleton<SkillDescription>
{
    public Text skillTitleName;
    public Text skillDescription;
    public Text skillCostDescription;

    public void ResetSkillDescription(Skill skill)
    {
        if (skill == null)
            return;

        SetSkillTitleName(skill.skillName);
        SetSkillDescription(skill.description);
        SetSkillCostDescription(skill.costType, skill.cost.Value);
    }

    public void SetSkillTitleName(string text)
    {
        skillTitleName.text = "【" + text + "】";
    }

    public void SetSkillDescription(string text)
    {
        skillDescription.text = text;
    }

    public void SetSkillCostDescription(CostType costType, float cost)
    {
        string typeText = string.Empty;
        switch (costType)
        {
            case CostType.Health:
                typeText = "生命";
                break;
            case CostType.Mana:
                typeText = "魔力";
                break;
        }
        skillCostDescription.text = cost.ToString() + "  " + typeText;
    }
}
