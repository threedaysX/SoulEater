using UnityEngine;
using UnityEngine.UI;

public class SkillDescription : Singleton<SkillDescription>
{
    public Text skillTitleName;
    public Text skillDescription;

    public void SetSkillTitleName(string text)
    {
        skillTitleName.text = text;
    }

    public void SetSkillDescription(string text)
    {
        skillDescription.text = text;
    }
}
