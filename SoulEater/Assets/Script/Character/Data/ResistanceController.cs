using StatsModel;
using System.Collections;
using UnityEngine;

public class ResistanceController : Singleton<ResistanceController>
{
    public void ResetResistanceData(Character target, ElementType elementType)
    {
        ResistanceData resistanceData = new ResistanceData(elementType);
        target.data.resistance.none = resistanceData.none;
        target.data.resistance.fire = resistanceData.fire;
        target.data.resistance.water = resistanceData.water;
        target.data.resistance.earth = resistanceData.earth;
        target.data.resistance.air = resistanceData.air;
        target.data.resistance.thunder = resistanceData.thunder;
        target.data.resistance.light = resistanceData.light;
        target.data.resistance.dark = resistanceData.dark;
    }

    public void ForceChangeTargetElement(Character target, ElementType elementType, float duration)
    {
        ResistanceData resistanceData = new ResistanceData(elementType);
        ForceChangeTargetElementValue(target, resistanceData, duration);
    }

    public void ForceChangeTargetElementValue(Character target, ResistanceData resistanceData, float duration)
    {
        StartCoroutine(ForceChangeTargetElementValueCor(target, resistanceData, duration));
    }

    private IEnumerator ForceChangeTargetElementValueCor(Character target, ResistanceData resistanceData, float duration)
    {
        target.data.resistance.none.ForceToChangeValue(resistanceData.none.Value);
        target.data.resistance.fire.ForceToChangeValue(resistanceData.fire.Value);
        target.data.resistance.water.ForceToChangeValue(resistanceData.water.Value);
        target.data.resistance.earth.ForceToChangeValue(resistanceData.earth.Value);
        target.data.resistance.air.ForceToChangeValue(resistanceData.air.Value);
        target.data.resistance.thunder.ForceToChangeValue(resistanceData.thunder.Value);
        target.data.resistance.light.ForceToChangeValue(resistanceData.light.Value);
        target.data.resistance.dark.ForceToChangeValue(resistanceData.dark.Value);
        yield return new WaitForSeconds(duration);
        target.data.resistance.none.CancelForceValue();
        target.data.resistance.fire.CancelForceValue();
        target.data.resistance.water.CancelForceValue();
        target.data.resistance.earth.CancelForceValue();
        target.data.resistance.air.CancelForceValue();
        target.data.resistance.thunder.CancelForceValue();
        target.data.resistance.light.CancelForceValue();
        target.data.resistance.dark.CancelForceValue();
    }
}

[System.Serializable]
public struct ResistanceData
{
    public Stats none;
    public Stats fire;
    public Stats water;
    public Stats earth;
    public Stats air;
    public Stats thunder;
    public Stats light;
    public Stats dark;

    public ResistanceData(Stats none, Stats fire, Stats water, Stats earth, Stats air, Stats thunder, Stats light, Stats dark)
    {
        this.none = none;
        this.fire = fire;
        this.water = water;
        this.earth = earth;
        this.air = air;
        this.thunder = thunder;
        this.light = light;
        this.dark = dark;
    }

    public ResistanceData(float none, float fire, float water, float earth, float air, float thunder, float light, float dark)
    {
        this.none = new Stats(none);
        this.fire = new Stats(fire);
        this.water = new Stats(water);
        this.earth = new Stats(earth);
        this.air = new Stats(air);
        this.thunder = new Stats(thunder);
        this.light = new Stats(light);
        this.dark = new Stats(dark);
    }

    public ResistanceData(ElementType elementType)
    {
        switch (elementType)
        {
            #region None
            case ElementType.None:
                none = new Stats(100);
                fire = new Stats(100);
                water = new Stats(100);
                earth = new Stats(100);
                air = new Stats(100);
                thunder = new Stats(100);
                light = new Stats(100);
                dark = new Stats(100);
                break;
            #endregion
            #region Fire
            case ElementType.Fire:
                none = new Stats(100);
                fire = new Stats(25);
                water = new Stats(200);
                earth = new Stats(50);
                air = new Stats(125);
                thunder = new Stats(100);
                light = new Stats(100);
                dark = new Stats(125);
                break;
            #endregion
            #region Water
            case ElementType.Water:
                none = new Stats(100);
                fire = new Stats(50);
                water = new Stats(25);
                earth = new Stats(100);
                air = new Stats(125);
                thunder = new Stats(200);
                light = new Stats(100);
                dark = new Stats(125);
                break;
            #endregion
            #region Earth
            case ElementType.Earth:
                none = new Stats(100);
                fire = new Stats(200);
                water = new Stats(100);
                earth = new Stats(25);
                air = new Stats(100);
                thunder = new Stats(25);
                light = new Stats(100);
                dark = new Stats(125);
                break;
            #endregion
            #region Air
            case ElementType.Air:
                none = new Stats(100);
                fire = new Stats(100);
                water = new Stats(100);
                earth = new Stats(100);
                air = new Stats(25);
                thunder = new Stats(50);
                light = new Stats(100);
                dark = new Stats(125);
                break;
            #endregion
            #region Thunder
            case ElementType.Thunder:
                none = new Stats(100);
                fire = new Stats(100);
                water = new Stats(50);
                earth = new Stats(200);
                air = new Stats(50);
                thunder = new Stats(25);
                light = new Stats(100);
                dark = new Stats(125);
                break;
            #endregion
            #region Light
            case ElementType.Light:
                none = new Stats(100);
                fire = new Stats(100);
                water = new Stats(100);
                earth = new Stats(100);
                air = new Stats(100);
                thunder = new Stats(100);
                light = new Stats(50);
                dark = new Stats(200);
                break;
            #endregion
            #region Dark
            case ElementType.Dark:
                none = new Stats(100);
                fire = new Stats(125);
                water = new Stats(125);
                earth = new Stats(125);
                air = new Stats(125);
                thunder = new Stats(125);
                light = new Stats(200);
                dark = new Stats(200);
                break;
            #endregion
            #region All Zero (Default & Empty)
            case ElementType.Empty:
            default:
                none = new Stats(0);
                fire = new Stats(0);
                water = new Stats(0);
                earth = new Stats(0);
                air = new Stats(0);
                thunder = new Stats(0);
                light = new Stats(0);
                dark = new Stats(0);
                break;
            #endregion
        }
    }
}
