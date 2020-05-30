using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Dictionary<WeaponType, int> weaponDictionary = new Dictionary<WeaponType, int>
    {
        { WeaponType.None, 1 },
        { WeaponType.OneHandSword, 2 },
        { WeaponType.OneHandAx, 3 },
        { WeaponType.OneHandSpear, 3 },
        { WeaponType.TwoHandSword, 3 },
        { WeaponType.TwoHandAx, 2 },
        { WeaponType.TwoHandSpear, 3 },
        { WeaponType.TwoHandMallet, 4 },
        { WeaponType.TwoHandScythe, 2 },
        { WeaponType.Dagger, 4 },
        { WeaponType.Rapier, 4 },
        { WeaponType.Katana, 3 },
        { WeaponType.Bow, 1 },
        { WeaponType.Whip, 2 }
    };

    private Character character;
    private WeaponType lastWeaponType;
    public bool isDirtyWeaponData = false;

    private void Start()
    {
        character = GetComponent<Character>();
        isDirtyWeaponData = true;
    }

    private void Update()
    {
        if (isDirtyWeaponData || character.data.weaponType != lastWeaponType)
        {
            ResetCharacterWeaponData(character);
            lastWeaponType = character.data.weaponType;
            isDirtyWeaponData = false;
        }
    }

    /// <summary>
    /// 各種武器的攻擊次數 (每個循環)
    /// </summary>
    public int GetWeaponAttackCount(WeaponType weaponType)
    {
        return weaponDictionary[weaponType];
    }

    public void ResetCharacterWeaponData(Character character)
    {
        character.data.cycleAttackCount = GetWeaponAttackCount(character.data.weaponType);
        if (character.anim != null)
        {
            character.anim.SetInteger("WeaponType", (int)character.data.weaponType);
        }
    }

}

public enum WeaponType
{
    None,           
    OneHandSword,   // {3}、{4} (占用武器格數)
    OneHandAx,      // {3}、{4}
    OneHandSpear,   // {3}、{4}
    OneHandMallet,  // {3}、{4}
    TwoHandSword,   // {6}
    TwoHandAx,      // {6}
    TwoHandSpear,   // {6}
    TwoHandMallet,  // {6}
    TwoHandScythe,  // {6}
    Dagger,         // {3}
    Rapier,         // {4}、{6}
    Katana,         // {6}
    Bow,            // {6}
    Whip,           // {4}、{6}
    Shield          // {2}、{3}、{4}
}
