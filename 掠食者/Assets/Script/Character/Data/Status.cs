using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Status")]
public class Status : ScriptableObject
{
    public Stats strength;
    public Stats agility;
    public Stats vitality;
    public Stats dexterity;
    public Stats intelligence;
    public Stats lucky;
}
