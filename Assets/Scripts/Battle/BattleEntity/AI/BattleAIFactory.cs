using System.Linq;
using System.Collections.Generic;
namespace Doragon.Battle
{
    public class BattleAIFactory
    {
        //TODO: add more parameters besides identifier for creation

        /// <summary>
        /// Manufactures <see cref="IBattleAI"> according to classID
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>Returns an implementor of <see cref="IBattleAI">, else null.</returns>
        public IBattleAI Build(string classID, IEnumerable<IBattleEntity> opposingTeam)
        {
            IBattleAI battleAI = null;
            switch (classID)
            {
                case "Wulf":
                    battleAI = new WulfAI(opposingTeam.ToList());
                    break;
            }
            return battleAI;
        }
    }
}