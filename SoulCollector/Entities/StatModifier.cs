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

        public StatModifier(ModifierType type, int value)
        {
            _value = value;
            _type = type;
        }

        public int GetValue(int baseValue)
        {
            switch (_type)
            {
                case ModifierType.Flat:
                    return _value;
                case ModifierType.Percentage:
                    return (int)(_value / 100.0f * baseValue);
                default:
                    return 0;
            }
        }
    }
}