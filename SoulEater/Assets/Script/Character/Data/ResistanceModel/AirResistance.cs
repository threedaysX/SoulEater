using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Resistance/Air")]
public class AirResistance : Resistance
{
    private void Awake()
    {
        none = new Stats(100);
        fire = new Stats(100);
        water = new Stats(100);
        earth = new Stats(100);
        air = new Stats(25);
        thunder = new Stats(50);
        light = new Stats(100);
        dark = new Stats(125);
    }
}
