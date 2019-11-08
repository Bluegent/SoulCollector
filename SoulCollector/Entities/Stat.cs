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

    public class Stat
    {
        public StatType Type { get; private set; }
        private int _base;

        public int Value { get; private set; }
        private List<StatModifier> _modifiers;

        public Stat(StatType type, int baseValue)
        {
            _modifiers = new List<StatModifier>();
            Type = type;
            _base = baseValue;
            Recalculate();
        }

        private void Recalculate()
        {
            Value = _base;
            foreach (StatModifier mod in _modifiers)
            {
                Value += mod.GetValue(_base);
            }
        }

        public void AddModifier(StatModifier mod)
        {
            _modifiers.Add(mod);
            Recalculate();
        }

        public void RemoveModifier(StatModifier mod)
        {
            _modifiers.Remove(mod);
        }
    }
}