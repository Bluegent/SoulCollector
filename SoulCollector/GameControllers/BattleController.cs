using SoulCollector.Entities;

namespace SoulCollector.GameControllers
{
    using System;
    using System.Text;

    using SoulCollector.Output;


    public class Party
    {
        public Entity[] Members;
        public Party(Entity[] members)
        {
            Members = members;
        }

        public bool IsAlive()
        {
            foreach (Entity ent in Members)
            {
                if (ent.IsAlive())
                {
                    return true;
                }

            }

            return false;
        }

        public void UpdateParty(long gameTick)
        {
            foreach (Entity ent in Members)
            {
                ent.Update(gameTick);
            }
        }


        public string GetWinText()
        {
            StringBuilder bld = new StringBuilder(ToString());
            if (Members.Length == 1)
                bld.Append(" has won");
            else
                bld.Append(" have won.");
            return bld.ToString();
        }

        public override string ToString()
        {
            StringBuilder bld = new StringBuilder();


            for (int i = 0; i < Members.Length - 2; ++i)
            {
                bld.Append(Members[i].Name);
                bld.Append(", ");
            }

            if (Members.Length >= 2)
            {
                bld.Append(Members[Members.Length - 2].Name);
                bld.Append(" and ");
            }

            bld.Append(Members[Members.Length - 1].Name);
            return bld.ToString();
        }
    }

    public class BattleController
    {
        private Party _party1;

        private Party _party2;

        private static long maxTicks = 100000;

        private ILogger _log;

        public BattleController(Party party1, Party party2, Dependencies dep)
        {


            _party1 = party1;
            _party2 = party2;
            _log = dep.Log;

            _log.Log($"{party1} has entered battle with {party2}.");
            foreach (Entity ent in _party1.Members)
            {
                ent.EnterBattle(_party1.Members, _party2.Members);
            }

            foreach (Entity ent in _party2.Members)
            {
                ent.EnterBattle(_party2.Members, _party1.Members);
            }
        }



        public enum BattleOutcome
        {
            Party1Win,
            Party2Win,
            Draw
        }
        private BattleOutcome GetWinner()
        {
            bool party1Alive = _party1.IsAlive();
            bool party2Alive = _party2.IsAlive();
            if (party1Alive && !party2Alive)
                return BattleOutcome.Party1Win;
            if (party2Alive && !party1Alive)
                return BattleOutcome.Party2Win; ;
            return BattleOutcome.Draw;
        }

        public BattleOutcome BattleLoop()
        {
            long battleTick = 0;
            while (battleTick <= maxTicks && _party1.IsAlive() && _party2.IsAlive())
            {
                _party1.UpdateParty(battleTick);
                _party2.UpdateParty(battleTick);
                ++battleTick;
            }

            BattleOutcome winner = GetWinner();
            switch (winner)
            {
                case BattleOutcome.Party1Win:
                    _log.Log(_party1.GetWinText());
                    break;
                case BattleOutcome.Party2Win:
                    _log.Log(_party2.GetWinText());
                    break;
                case BattleOutcome.Draw:
                    _log.Log("Battle ended in a draw.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return winner;
        }
    }
}