using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Resistance/None")]
public class NoneResistance : Resistance
{
    private void Awake()
    {
        none = new Stats(100);
        fire = new Stats(100);
        water = new Stats(100);
        earth = new Stats(100);
        air = new Stats(100);
        thunder = new Stats(100);
        light = new Stats(100);
        dark = new Stats(100);
    }
}
