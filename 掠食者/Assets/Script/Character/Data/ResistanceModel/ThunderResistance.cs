using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Resistance/Thunder")]
public class ThunderResistance : Resistance
{
    private void Awake()
    {
        none = new Stats(100);
        fire = new Stats(100);
        water = new Stats(50);
        earth = new Stats(200);
        air = new Stats(50);
        thunder = new Stats(25);
        light = new Stats(100);
        dark = new Stats(125);
    }
}
