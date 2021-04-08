using System;
using System.Collections.Generic;
namespace Doragon.Battle
{
    public abstract class IBattleAI
    {
        public Dictionary<IBattleEntity, int> entityEnmity = new Dictionary<IBattleEntity, int>();
        private bool tripleMove = false;
        private Random rand = new Random();
        protected IBattleAI(List<IBattleEntity> opposingTeam, bool isDragon)
        {
            opposingTeam.ForEach(t => entityEnmity.Add(t, 0));
            tripleMove = isDragon;
        }

        public abstract Targets SelectTargets();

        public int Rand(int inclusive, int exclusive)
        {
            return rand.Next(inclusive, exclusive);
        }
    }
}