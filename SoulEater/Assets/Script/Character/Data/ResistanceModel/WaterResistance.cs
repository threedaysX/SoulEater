using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Resistance/Water")]
public class WaterResistance : Resistance
{
    private void Awake()
    {
        none = new Stats(100);
        fire = new Stats(50);
        water = new Stats(25);
        earth = new Stats(100);
        air = new Stats(125);
        thunder = new Stats(200);
        light = new Stats(100);
        dark = new Stats(125);
    }
}
