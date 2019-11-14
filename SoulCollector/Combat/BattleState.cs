using SoulCollector.Entities;

namespace SoulCollector.Combat
{
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
}