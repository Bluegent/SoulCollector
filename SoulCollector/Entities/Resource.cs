using System.Data;

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
        public ResourceType Type { get; private set; }


        public Resource(ResourceType type, int startValue, int maxValue)
        {
            Current = startValue;
            BaseMaxValue = maxValue;
            Type = type;
            Recalculate();
        }

        public void Recalculate(long tickTime)
        {
            Current = Utils.Math.Clamp(Current, 0, MaxValue);
        }

        protected sealed override void Recalculate()
        {
            MaxValue = BaseMaxValue;

            foreach (StatModifier mod in _modifiers)
            {
                MaxValue += mod.GetValue(BaseMaxValue);
            }
        }
    }
}