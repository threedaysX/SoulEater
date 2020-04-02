using System;
using System.Linq;
using System.Collections.Generic;
using StatsModifierModel;
using UnityEngine;

namespace StatsModel
{
    [Serializable]
    public class Stats
    {
        public float BaseValue;
        public float AdditionalValue;
        public float Value
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue || AdditionalValue != lastAdditionalValue)
                {
                    lastBaseValue = BaseValue;
                    lastAdditionalValue = AdditionalValue;
                    ResetDirtyFinalValue();
                }
                return FinalValue;
            }
        }

        [ReadOnly][SerializeField] protected float FinalValue;
        protected float lastBaseValue = float.MinValue;
        protected float lastAdditionalValue = float.MinValue;
        protected bool isDirty = true;
        public List<StatModifier> modifiers = new List<StatModifier>();

        public Stats()
        {
            modifiers = new List<StatModifier>();
        }
        public Stats(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        private float GetFinalValue()
        {
            float finalValue = BaseValue + AdditionalValue;
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
                    case StatModType.Magnification:
                        timesMod *= (1 + (mod.Value / 100));
                        break;
                    case StatModType.MagnificationAdd:
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
        public void AddModifier(StatModifier mod)
        {
            if (mod.Value != 0)
            {
                modifiers.Add(mod);
                ResetDirtyFinalValue();
            }
        }

        public bool RemoveModifier(StatModifier mod)
        {
            if (mod.Value != 0)
            {
                var getTrueModInList = modifiers.FirstOrDefault(item => item.SourceName.Equals(mod.SourceName) && item.Type.Equals(mod.Type) && item.Value.Equals(mod.Value));
                if (modifiers.Remove(getTrueModInList))
                {
                    ResetDirtyFinalValue();
                    return true;
                }
            }
            return false;
        }

        private void ResetDirtyFinalValue()
        {
            FinalValue = GetFinalValue();
            isDirty = false;
        }
    }
}
