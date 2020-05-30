using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Status")]
public class Status : ScriptableObject
{
    public Stats strength = new Stats(1);
    public Stats agility = new Stats(1);
    public Stats vitality = new Stats(1);
    public Stats dexterity = new Stats(1);
    public Stats intelligence = new Stats(1);
    public Stats lucky = new Stats(1);
}
