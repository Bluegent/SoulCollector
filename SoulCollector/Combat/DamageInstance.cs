namespace SoulCollector.Combat
{
    public enum DamageType
    {
        Physical,
        Fire,
        Water,
        Earth,
        Light,
        Darkness,
        True
    }
    public class DamageInstance
    {
        public int Amount;
        public DamageType Type;

        public override string ToString()
        {
            return $"{Amount} {Type} Damage";
        }
    }
}