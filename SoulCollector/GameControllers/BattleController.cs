using SoulCollector.Entities;

namespace SoulCollector.GameControllers
{
    public class BattleController
    {
        private Entity[] _party1;
        private Entity[] _party2;

        public BattleController(Entity[] party1, Entity[] party2)
        {
            _party1 = party1;
            _party2 = party2;
            foreach (Entity ent in _party1)
            {
                ent.EnterBattle(_party1, _party2);
            }
            foreach (Entity ent in _party2)
            {
                ent.EnterBattle(
                    _party2, _party1);
            }
        }

        private bool isPartyAlive(Entity[] party)
        {
            foreach (Entity ent in party)
            {
                if (ent.IsAlive())
                {
                    return true;
                }

            }
            return false;
        }

        public void BattleLoop()
        {
            while (isPartyAlive(_party1) && isPartyAlive(_party2))
            {

            }
        }
    }
}