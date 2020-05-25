using UnityEngine;

/// <summary>
/// 詞綴 (Model)
/// </summary>
[CreateAssetMenu(menuName = "Endowment/AffixModel")]
public class Affix : ScriptableObject
{
    public string description;
    public AffixData affixData;
}