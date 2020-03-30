using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Status")]
public class Status : ScriptableObject
{
    public Stats strength = new Stats(5);
    public Stats agility = new Stats(5);
    public Stats vitality = new Stats(5);
    public Stats dexterity = new Stats(5);
    public Stats intelligence = new Stats(5);
    public Stats lucky = new Stats(5);
}
