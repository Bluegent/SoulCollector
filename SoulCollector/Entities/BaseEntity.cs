using System.Collections.Generic;
using SoulCollector.Combat;
using SoulCollector.GameControllers;

namespace SoulCollector.Entities
{
    public class BaseEntity : Entity
    {
        protected Dictionary<StatType, Stat> Stats;
        protected Dictionary<ResourceType, Resource> Resources;
        private AttackDamage _physicalDamage;

        public BaseEntity(Dependencies dep) : base(dep)
        {
            Stats = new Dictionary<StatType, Stat>();
            Stat delay = new Stat(StatType.AttackDelay, 100);
            Stats.Add(delay.Type, delay);
            _physicalDamage = new AttackDamage(DamageType.Physical, 10, 20);
            Resources = new Dictionary<ResourceType, Resource>();
            Resource health = new Resource(ResourceType.Health, 100, 100);
            Resources.Add(health.Type, health);
        }

        public override string ToString()
        {
            return $"{Name} - {Resources[ResourceType.Health]}";
        }

        protected override void AttackImpl(Entity targets)
        {
            targets.TakeDamage(_physicalDamage.GetDamageInstance(), this);

        }

        protected override void TakeDamageImpl(DamageInstance damage, Entity source)
        {
            if (!IsAlive())
                return;
            Resources[ResourceType.Health].Modify(damage.Amount);
            Log.Log($"{this} took {damage} from {source.Name}");
            if (!IsAlive())
                Log.Log($"{Name} has died.");
        }

        public override void Update(long gameTick)
        {
            if (!IsAlive())
                return;
            if (gameTick >= State.NextAttackTick)
            {
                int target = Utils.Math.Random(0, State.Enemies.Length);
                AttackImpl(State.Enemies[target]);
                State.NextAttackTick += Stats[StatType.AttackDelay];
            }
        }

        public override bool IsAlive()
        {
            return Resources[ResourceType.Health].Current > 0;
        }
    }
}