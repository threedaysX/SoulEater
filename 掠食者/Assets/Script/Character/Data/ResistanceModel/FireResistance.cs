using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Resistance/Fire")]
public class FireResistance : Resistance
{
    private void Awake()
    {
        none = new Stats(100);
        fire = new Stats(25);
        water = new Stats(200);
        earth = new Stats(50);
        air = new Stats(125);
        thunder = new Stats(100);
        light = new Stats(100);
        dark = new Stats(125);
    }
}
