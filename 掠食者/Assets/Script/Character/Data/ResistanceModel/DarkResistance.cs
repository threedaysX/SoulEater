using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Resistance/Dark")]
public class DarkResistance : Resistance
{
    private void Awake()
    {
        none = new Stats(100);
        fire = new Stats(125);
        water = new Stats(125);
        earth = new Stats(125);
        air = new Stats(125);
        thunder = new Stats(125);
        light = new Stats(200);
        dark = new Stats(200);
    }
}
