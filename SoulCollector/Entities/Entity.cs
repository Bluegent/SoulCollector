using System.Collections.Generic;

namespace SoulCollector.Entities
{

    public interface IEffect
    {
        void Apply(Entity source, Entity target);
    }

    public abstract class Entity
    {
        public List<IEffect> BeforeAttackEffects;
        public List<IEffect> AttackEffects;
        public List<IEffect> AfterAttackEffects;
        public List<IEffect> BeforeHitEffects;
        public List<IEffect> HitEffects;
        public List<IEffect> AfterHitEffects;


        public Entity()
        {
            BeforeAttackEffects = new List<IEffect>();
            AttackEffects = new List<IEffect>();
            AfterAttackEffects = new List<IEffect>();
            BeforeHitEffects = new List<IEffect>();
            HitEffects = new List<IEffect>();
            AfterHitEffects = new List<IEffect>();
        }


        private void RunEffects(List<IEffect> effects, Entity[] targets)
        {
            foreach (IEffect effect in effects)
                foreach (Entity target in targets)
                    effect.Apply(this, target);
        }

        public void BeforeAttackState(Entity[] targets)
        {
            RunEffects(BeforeHitEffects,targets);
        }

        public void AttackState(Entity[] targets)
        {
            RunEffects(AttackEffects, targets);
        }

        public void AfterAttackState(Entity[] targets)
        {
            RunEffects(AfterAttackEffects, targets);
        }

        public void BeforeDamagedState(Entity[] targets)
        {
            RunEffects(BeforeHitEffects, targets);
        }

        public void DamagedState(Entity[] targets)
        {
            RunEffects(HitEffects, targets);
        }

        public void AfterDamagedState(Entity[] targets)
        {
            RunEffects(AfterHitEffects, targets);
        }

        public abstract void Update(long gameTick);
    }
    public class BaseEntity
    {
        protected Dictionary<StatType, Stat> Stats;
        protected Dictionary<ResourceType, Resource> Resources;

        public BaseEntity()
        {
            Stats = new Dictionary<StatType, Stat>();
            Resources = new Dictionary<ResourceType, Resource>();
            Resource health = new Resource(ResourceType.Health, 100, 100);
            Resources.Add(health.Type, health);
        }

    }
}