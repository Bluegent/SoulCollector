using System.Collections.Generic;
using SoulCollector.Combat;

namespace SoulCollector.Entities
{

    using GameControllers;
    using Output;

    public abstract class Entity
    {
        protected CombatEvent _hitState;
        protected CombatEvent _attackState;
        protected CombatEvent _battleState;

        public string Name;
        public ILogger Log;
        protected BattleState State;

        public BattleState GetState()
        {
            return State;
        }
        protected Entity(Dependencies dep)
        {
            Log = dep.Log;
            _hitState = new CombatEvent(this);
            _attackState = new CombatEvent(this);
            _battleState = new CombatEvent(this);
            State = new BattleState();
            Name = "";

        }

        public void EnterBattle(Entity[] allies, Entity[] enemies)
        {
            if (State.IsInBattle)
                return;
            State.Enemies = enemies;
            List<Entity> party = new List<Entity>(allies);
            party.Remove(this);
            State.Allies = party.ToArray();
            _battleState.Pre.ExecuteEffects(State.Allies, State.Enemies);
            State.IsInBattle = true;
        }

        public void LeaveBattle()
        {
            if (!State.IsInBattle)
                return;
            _battleState.Post.ExecuteEffects(State.Allies, State.Enemies);
            State.Reset();
        }

        public void Attack(Entity[] targets)
        {
            _attackState.Pre.ExecuteEffects(State.Allies, State.Enemies);
            foreach (Entity target in targets)
            {
                AttackImpl(target);
            }

            _attackState.CombatStage.ExecuteEffects(State.Allies, State.Enemies);
            _attackState.Post.ExecuteEffects(State.Allies, State.Enemies);
        }

        public void TakeDamage(DamageInstance damage, Entity source)
        {
            _hitState.Pre.ExecuteEffects(State.Allies, State.Enemies);
            TakeDamageImpl(damage, source);
            _hitState.CombatStage.ExecuteEffects(State.Allies, State.Enemies);
            _hitState.Post.ExecuteEffects(State.Allies, State.Enemies);
        }

        protected abstract void AttackImpl(Entity target);
        protected abstract void TakeDamageImpl(DamageInstance damage, Entity source);
        public abstract void Update(long gameTick);
        public abstract bool IsAlive();
    }
}