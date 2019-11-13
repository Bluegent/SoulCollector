using System.Collections.Generic;
using System.Net.Mail;

namespace SoulCollector.Entities
{

    public class Stage
    {
        private Entity _parent;
        private List<IEffect> _effects;

        public Stage(Entity parent)
        {
            _parent = parent;
            _effects = new List<IEffect>();
        }

        public void AddEffect(IEffect effect)
        {
            _effects.Add(effect);
        }

        public void RemoveEffect(IEffect effect)
        {
            _effects.Remove(effect);
        }

        public void ExecuteEffects(Entity[] targets)
        {
            foreach (Entity target in targets)
                ExecuteEffects(target);
        }

        public void ExecuteEffects(Entity target)
        {
            foreach (IEffect effect in _effects)
                effect.Apply(_parent, target);
        }
    }

    public class CombatEvent
    {
        public Stage Pre { get; }
        public Stage Stage { get; }
        public Stage Post { get; }

        public CombatEvent(Entity parent)
        {
            Pre = new Stage(parent);
            Stage = new Stage(parent);
            Post = new Stage(parent);
        }

    }


    public class DamageInstance
    {
        public int Amount;
    }

    public interface IEffect
    {
        void Apply(Entity source, Entity target);
    }

    public abstract class Entity
    {
        protected CombatEvent _hitState;
        protected CombatEvent _attackState;
        protected CombatEvent _battleState;

        public Entity()
        {
            _hitState = new CombatEvent(this);
            _attackState = new CombatEvent(this);
            _battleState = new CombatEvent(this);
        }

        public void EnterBattle(Entity[] targets)
        {
            _battleState.Pre.ExecuteEffects(targets);
        }

        public void LeaveBattle(Entity[] targets)
        {
            _battleState.Post.ExecuteEffects(targets);
        }

        public void Attack(Entity[] targets)
        {
            _attackState.Pre.ExecuteEffects(targets);
            AttackImpl(targets);
            _attackState.Stage.ExecuteEffects(targets);
            _attackState.Post.ExecuteEffects(targets);
        }

        protected abstract void AttackImpl(Entity[] targets);


        public void TakeDamage(DamageInstance damage, Entity source)
        {
            _hitState.Pre.ExecuteEffects(source);
            TakeDamageImpl(damage, source);
        }

        protected abstract void TakeDamageImpl(DamageInstance damage, Entity source);
        public abstract void Update(long gameTick);
    }

    public class BaseEntity : Entity
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

        protected override void AttackImpl(Entity[] targets)
        {
            foreach (Entity entity in targets)
            {
                entity.TakeDamage(new DamageInstance() { Amount = 10 }, this);
            }
        }

        protected override void TakeDamageImpl(DamageInstance damage, Entity source)
        {
            Resources[ResourceType.Health].Modify(damage.Amount);
        }

        public override void Update(long gameTick)
        {
            throw new System.NotImplementedException();
        }
    }

}