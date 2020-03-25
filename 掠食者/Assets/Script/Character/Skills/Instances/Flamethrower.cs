using UnityEngine;
using StatsModifierModel;

[CreateAssetMenu(menuName = "Character/Skill/Flamethrower")]
public class Flamethrower : Skill
{
    /// <summary>
    /// 擊退效果
    /// </summary>
    public void KnockBack()
    {
        character.transform.position -= new Vector3(character.data.moveSpeed.Value * Time.deltaTime, 0, 0);
        foreach (var hit in hits)
        {
            hit.transform.position -= new Vector3(hit.GetComponent<Character>().data.moveSpeed.Value * Time.deltaTime, 0, 0);
        }
    }

    /// <summary>
    /// 增加敵人火炕、減少敵人火炕
    /// </summary>
    public void ImproveFireResistance()
    {
        // 自身+20%火炕，持續5秒
        BuffController.Instance.AddModifier(character.data.resistance.fire, new StatModifier(20, StatModType.FlatAdd), 5f);

        // 敵人-20%火炕，持續5秒
        foreach (var hit in hits)
        {
            var enemyFireResistance = hit.GetComponent<Character>().data.resistance.fire;
            BuffController.Instance.AddModifier(enemyFireResistance, new StatModifier(-20, StatModType.FlatAdd), 5f);
        }
    }
}
