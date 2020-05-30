using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Resistance/Light")]
public class LightResistance : Resistance
{
    private void Awake()
    {
        none = new Stats(100);
        fire = new Stats(100);
        water = new Stats(100);
        earth = new Stats(100);
        air = new Stats(100);
        thunder = new Stats(100);
        light = new Stats(50);
        dark = new Stats(200);
    }
}
