using SoulCollector.Entities;

namespace SoulCollector.SoulEffects
{

    public interface IEffect
    {
        void ApplyEnemy(Entity source, Entity enemy);
        void ApplyAlly(Entity source, Entity ally);
    }
}