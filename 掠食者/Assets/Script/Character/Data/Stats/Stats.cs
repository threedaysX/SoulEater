using System.Collections.Generic;
using System;
using StatsModifierModel;
using UnityEngine;

namespace StatsModel
{
    [Serializable]
    public class Stats
    {
        private List<StatModifier> modifiers;
        private bool isDirty = true;

        [SerializeField]
        public float BaseValue;
        private float FinalValue;
        public float Value
        {
            get
            {
                if (isDirty)
                {
                    FinalValue = GetFinalValue();
                    isDirty = false;
                }
                return FinalValue;
            }
        }

        public Stats(float baseValue)
        {
            BaseValue = baseValue;
            modifiers = new List<StatModifier>();
        }

        private float GetFinalValue()
        {
            float finalValue = BaseValue;
            float addMod = 0;
            float timesMod = 1;
            float timesOfAddMod = 0;

            foreach (var mod in modifiers)
            {
                switch (mod.Type)
                {
                    case StatModType.FlatAdd:
                        addMod += mod.Value;
                        break;
                    case StatModType.Times:
                        timesMod *= (1 + mod.Value);
                        break;
                    case StatModType.TimesOfAdd:
                        timesOfAddMod += mod.Value;
                        break;
                    case StatModType.PercentMult:
                        timesMod *= (1 + (mod.Value / 100));
                        break;
                    case StatModType.PercentAdd:
                        timesOfAddMod += (mod.Value / 100);
                        break;
                }
            }

            // 數值變化皆為: 先[加減算] 後[乘算]
            if (addMod != 0)
            {
                finalValue += addMod;
            }
            if (timesOfAddMod != 0)
            {
                finalValue *= (1 + timesOfAddMod);
            }
            if (timesMod != 1)
            {
                finalValue *= timesMod;
            }

            // 取至小數點後兩位
            return (float)Math.Round(finalValue, 2);
        }

        /// <summary>
        /// 增、減能力。
        /// </summary>
        /// <param name="mod">增加的數值</param>
        /// <param name="duration">Buff持續多久的時間</param>
        public void AddModifier(StatModifier mod)
        {
            if (mod.Value != 0)
            {
                isDirty = true;
                modifiers.Add(mod);
            }
        }

        public bool RemoveModifier(StatModifier mod)
        {
            if (mod.Value != 0)
            {
                if (modifiers.Remove(mod))
                {
                    isDirty = true;
                    return true;
                }
            }
            return false;
        }
    }
}
