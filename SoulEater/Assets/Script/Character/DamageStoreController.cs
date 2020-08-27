using System.Collections.Concurrent;
using UnityEngine;

public class DamageStoreController : MonoBehaviour
{
    public const string Cumulative_DamageTake_KnockBack = "Cumulative_DamageTake_KnockBack";    // 累積受到的擊退傷害
    public const string Cumulative_DamageDealt_ManaAbsorb = "Cumulative_DamageDealt_ManaAbsorb";   // 累積造成的吸魔傷害

    private ConcurrentDictionary<string, int> damageTakeStoreDatas;
    private ConcurrentDictionary<string, int> damageDealtStoreDatas;

    // Start is called before the first frame update
    private void Start()
    {
        damageTakeStoreDatas = new ConcurrentDictionary<string, int>();
        damageDealtStoreDatas = new ConcurrentDictionary<string, int>();
        AddDamageStoreData(Cumulative_DamageTake_KnockBack, DamageStoreType.Take, 0);
        AddDamageStoreData(Cumulative_DamageDealt_ManaAbsorb, DamageStoreType.Dealt, 0);
    }

    public void DamageStore(DamageStoreType type, int damageTake)
    {
        var datas = GetStoreDataDict(type);
        foreach (var item in datas)
        {
            datas[item.Key] += damageTake;
        }
    }

    public int GetDamageData(string name, DamageStoreType type)
    {
        var datas = GetStoreDataDict(type);
        if (datas.TryGetValue(name, out int resultData))
        {
            return resultData;
        }
        return 0;
    }

    public void ModifyDamageData(string name, DamageStoreType type, int newDamageData)
    {
        GetStoreDataDict(type)[name] = newDamageData;
    }

    public void AddDamageStoreData(string name, DamageStoreType type, int defaultValue)
    {
        var datas = GetStoreDataDict(type);
        if (!datas.ContainsKey(name))
            datas.TryAdd(name, defaultValue);
    }

    public void RemoveDamageStoreData(string name, DamageStoreType type)
    {
        var datas = GetStoreDataDict(type);
        if (datas.ContainsKey(name))
            datas.TryRemove(name, out _);
    }

    private ConcurrentDictionary<string, int> GetStoreDataDict(DamageStoreType type)
    {
        switch (type)
        {
            case DamageStoreType.Take:
                return damageTakeStoreDatas;
            case DamageStoreType.Dealt:
                return damageDealtStoreDatas;
        }
        return null;
    }
}

public struct DamageStoreData 
{
    public DamageStoreType type;
    public int value;
}


public enum DamageStoreType
{
    Take,
    Dealt
}
