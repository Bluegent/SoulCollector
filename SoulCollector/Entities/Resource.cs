﻿using System.Data;

namespace SoulCollector.Entities
{

    public enum ResourceType
    {
        Health
    }
    public class Resource : ModifiableValue
    {
        public int Current { get; private set; }
        public int BaseMaxValue { get; private set; }
        public int MaxValue { get; private set; }
        public ResourceType Type { get; }


        public void Modify(int amount)
        {
            Current -= amount;
            Utils.Math.Clamp(Current, 0, MaxValue);
        }

        public void SetBaseValue(int value)
        {
            BaseMaxValue = value;
            Recalculate();
        }

        public Resource(ResourceType type, int startValue, int maxValue)
        {
            Current = startValue;
            BaseMaxValue = maxValue;
            Type = type;
            Recalculate();
        }

        protected sealed override void Recalculate()
        {
            MaxValue = BaseMaxValue;

            foreach (StatModifier mod in _modifiers)
            {
                MaxValue += mod.GetValue(BaseMaxValue);
            }
            Current = Utils.Math.Clamp(Current, 0, MaxValue);
        }
    }
}