namespace SoulCollector.Entities
{
    using System.Collections.Generic;

    public enum StatType
    {
        Attack, //flat damage add
        Defense, //flat damage reduction
        Critical, //chance to do a critical strike
        CriticalMultiplier, //damage multiplier for criticals
        DodgeChance, //chance to dodge attacks

    }

    public abstract class ModifiableValue
    {
        protected List<StatModifier> _modifiers;

        public ModifiableValue()
        {
            _modifiers = new List<StatModifier>();
        }

        protected abstract void Recalculate();

        public void AddModifier(StatModifier mod)
        {
            _modifiers.Add(mod);
            Recalculate();
        }

        public void RemoveModifier(StatModifier mod)
        {
            _modifiers.Remove(mod);
            Recalculate();

        }
    }

    public class Stat : ModifiableValue
    {
        public StatType Type { get; private set; }
        public int Base { get; private set; }

        public int Value { get; private set; }


        public static implicit operator int(Stat s)
        {
            return s.Value;
        }

        public Stat(StatType type, int baseValue)
        {

            Type = type;
            Base = baseValue;
            Recalculate();
        }

        protected sealed override void Recalculate()
        {
            Value = Base;
            foreach (StatModifier mod in _modifiers)
            {
                Value += mod.GetValue(Base);
            }
        }
    }
}