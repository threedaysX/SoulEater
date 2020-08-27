using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Resistance/Earth")]
public class EarthResistance : Resistance
{
    private void Awake()
    {
        none = new Stats(100);
        fire = new Stats(200);
        water = new Stats(100);
        earth = new Stats(25);
        air = new Stats(100);
        thunder = new Stats(25);
        light = new Stats(100);
        dark = new Stats(125);
    }
}
