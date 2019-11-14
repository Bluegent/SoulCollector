using System.Collections.Generic;

namespace SoulCollector.Entities
{
    using System.Runtime.Versioning;

    using SoulCollector.GameControllers;
    using SoulCollector.Output;

    public class Stage
    {
        private readonly Entity _parent;
        private readonly List<IEffect> _effects;

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

        public void ExecuteEffects(Entity[] allies, Entity[] enemies)
        {
            foreach (Entity target in allies)
                ExecuteForAlly(target);
            foreach (Entity target in enemies)
                ExecuteForEnemy(target);
        }

        public void ExecuteForAlly(Entity ally)
        {
            foreach (IEffect effect in _effects)
                effect.ApplyEnemy(_parent, ally);
        }

        public void ExecuteForEnemy(Entity enemy)
        {
            foreach (IEffect effect in _effects)
                effect.ApplyAlly(_parent, enemy);
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
        public DamageType Type;

        public override string ToString()
        {
            return $"{Amount} {Type} Damage";
        }
    }

    public interface IEffect
    {
        void ApplyEnemy(Entity source, Entity enemy);
        void ApplyAlly(Entity source, Entity ally);
    }


    public class BattleState
    {
        public Entity[] Allies;
        public Entity[] Enemies;

        public bool IsInBattle;
        public long NextAttackTick;


        public BattleState()
        {
            Reset();
            
        }

        public void Reset()
        {
            IsInBattle = false;
            Allies = null;
            Enemies = null;
            NextAttackTick = 0;
        }


    }

    public abstract class Entity
    {
        protected CombatEvent _hitState;
        protected CombatEvent _attackState;
        protected CombatEvent _battleState;

        public string Name;
        public ILogger Log;
        protected BattleState _state;


        public Entity(Dependencies dep)
        {
            Log = dep.Log;
            _hitState = new CombatEvent(this);
            _attackState = new CombatEvent(this);
            _battleState = new CombatEvent(this);
            _state = new BattleState();
            Name = "";

        }



        public void EnterBattle(Entity[] allies, Entity[] enemies)
        {
            if (_state.IsInBattle)
                return;
            _state.Enemies = enemies;
            List<Entity> party = new List<Entity>(allies);
            party.Remove(this);
            _state.Allies = party.ToArray();
            _battleState.Pre.ExecuteEffects(_state.Allies, _state.Enemies);
            _state.IsInBattle = true;
        }

        public void LeaveBattle()
        {
            if (!_state.IsInBattle)
                return;
            _battleState.Post.ExecuteEffects(_state.Allies, _state.Enemies);
            _state.Reset();
        }

        public void Attack(Entity[] targets)
        {
            _attackState.Pre.ExecuteEffects(_state.Allies, _state.Enemies);
            foreach (Entity target in targets)
            {
                AttackImpl(target);
            }

            _attackState.Stage.ExecuteEffects(_state.Allies, _state.Enemies);
            _attackState.Post.ExecuteEffects(_state.Allies, _state.Enemies);
        }

        protected abstract void AttackImpl(Entity target);


        public void TakeDamage(DamageInstance damage, Entity source)
        {
            _hitState.Pre.ExecuteEffects(_state.Allies, _state.Enemies);
            TakeDamageImpl(damage, source);
            _hitState.Stage.ExecuteEffects(_state.Allies, _state.Enemies);
            _hitState.Post.ExecuteEffects(_state.Allies, _state.Enemies);
        }

        protected abstract void TakeDamageImpl(DamageInstance damage, Entity source);
        public abstract void Update(long gameTick);
        public abstract bool IsAlive();
    }

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

    public class AttackDamage
    {
        public Stat Min { get; }
        public Stat Max { get; }
        public DamageType Type { get; }

        public AttackDamage(DamageType type, int baseMin, int baseMax)
        {
            Min = new Stat(StatType.Attack, baseMin);
            Max = new Stat(StatType.Attack, baseMax);
            Type = type;
        }

        public int Get()
        {
            if (Min.Value >= Max.Value)
            {
                return Max;
            }
            return Utils.Math.Random(Min, Max);
        }

        public DamageInstance GetDamageInstance()
        {
            DamageInstance result = new DamageInstance();
            result.Amount = Get();
            result.Type = Type;
            return result;
        }
    }


    public class BaseEntity : Entity
    {
        protected Dictionary<StatType, Stat> Stats;
        protected Dictionary<ResourceType, Resource> Resources;
        private AttackDamage _physicalDamage;

        public BaseEntity(Dependencies dep) : base(dep)
        {
            Stats = new Dictionary<StatType, Stat>();
            Stat delay = new Stat(StatType.AttackDelay,100);
            Stats.Add(delay.Type,delay);
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
            if(!IsAlive())
                Log.Log($"{Name} has died.");
        }

        public override void Update(long gameTick)
        {
            if (!IsAlive())
                return;
            if (gameTick >= _state.NextAttackTick)
            {
                int target = Utils.Math.Random(0, _state.Enemies.Length);
                AttackImpl(_state.Enemies[target]);
                _state.NextAttackTick += Stats[StatType.AttackDelay];
            }
        }

        public override bool IsAlive()
        {
            return Resources[ResourceType.Health].Current > 0;
        }
    }

}