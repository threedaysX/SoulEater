using StatsModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data/Resistance/Empty")]
public class Resistance : ScriptableObject
{
    public Stats fire;
    public Stats water;
    public Stats earth;
    public Stats air;
    public Stats thunder;
    public Stats light;
    public Stats dark;
}

[CreateAssetMenu(menuName = "Character/Data/Resistance/Fire")]
public class FireResistance : Resistance
{
    private void Awake()
    {
        fire = new Stats(25);
        water = new Stats(200);
        earth = new Stats(50);
        air = new Stats(125);
        thunder = new Stats(100);
        light = new Stats(100);
        dark = new Stats(125);
    }
}

[CreateAssetMenu(menuName = "Character/Data/Resistance/Water")]
public class WaterResistance : Resistance
{
    private void Awake()
    {
        fire = new Stats(50);
        water = new Stats(25);
        earth = new Stats(100);
        air = new Stats(125);
        thunder = new Stats(200);
        light = new Stats(100);
        dark = new Stats(125);
    }
}

[CreateAssetMenu(menuName = "Character/Data/Resistance/Earth")]
public class EarthResistance : Resistance
{
    private void Awake()
    {
        fire = new Stats(200);
        water = new Stats(100);
        earth = new Stats(25);
        air = new Stats(100);
        thunder = new Stats(25);
        light = new Stats(100);
        dark = new Stats(125);
    }
}

[CreateAssetMenu(menuName = "Character/Data/Resistance/Air")]
public class AirResistance : Resistance
{
    private void Awake()
    {
        fire = new Stats(100);
        water = new Stats(100);
        earth = new Stats(100);
        air = new Stats(25);
        thunder = new Stats(50);
        light = new Stats(100);
        dark = new Stats(125);
    }
}

[CreateAssetMenu(menuName = "Character/Data/Resistance/Thunder")]
public class ThunderResistance : Resistance
{
    private void Awake()
    {
        fire = new Stats(100);
        water = new Stats(50);
        earth = new Stats(200);
        air = new Stats(50);
        thunder = new Stats(25);
        light = new Stats(100);
        dark = new Stats(125);
    }
}

[CreateAssetMenu(menuName = "Character/Data/Resistance/Light")]
public class LightResistance : Resistance
{
    private void Awake()
    {
        fire = new Stats(100);
        water = new Stats(100);
        earth = new Stats(100);
        air = new Stats(100);
        thunder = new Stats(100);
        light = new Stats(50);
        dark = new Stats(200);
    }
}

[CreateAssetMenu(menuName = "Character/Data/Resistance/Dark")]
public class DarkResistance : Resistance
{
    private void Awake()
    {
        fire = new Stats(125);
        water = new Stats(125);
        earth = new Stats(125);
        air = new Stats(125);
        thunder = new Stats(125);
        light = new Stats(200);
        dark = new Stats(200);
    }
}