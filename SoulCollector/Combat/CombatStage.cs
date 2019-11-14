using System.Collections.Generic;
using SoulCollector.Entities;
using SoulCollector.SoulEffects;

namespace SoulCollector.Combat
{
    public class CombatStage
    {
        private readonly Entity _parent;
        private readonly List<IEffect> _effects;

        public CombatStage(Entity parent)
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

}