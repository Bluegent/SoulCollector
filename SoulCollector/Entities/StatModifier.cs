namespace SoulCollector.Entities
{

    public enum ModifierType
    {
        Flat,
        Percentage
    }

    public class StatModifier
    {
        private ModifierType _type;
        private int _value;

        public StatModifier(int value)
        {
            _value = value;
            _type = ModifierType.Flat;
        }

        public StatModifier(float value)
        {
            _value = (int)(value * 1000.0f);
            _type = ModifierType.Percentage;
        }

        public int GetValue(int baseValue)
        {
            switch (_type)
            {
                case ModifierType.Flat:
                    return _value;
                case ModifierType.Percentage:
                    return (int)(_value / 1000.0f * baseValue);
                default:
                    return 0;
            }
        }
    }
}