using System.Collections.Generic;
using System.Linq;
using Doragon.Logging;
namespace Doragon.Battle
{
    public class WulfAI : IBattleAI
    {
        // List<KeyValuePair<IBattleEntity, int>> entityEnmity
        public WulfAI(List<IBattleEntity> opposingTeam) : base(opposingTeam, isDragon: false)
        {

        }

        public void EnmityFromDamage(DamageRequest damageRequest)
        {
            entityEnmity[damageRequest.source] += 5;
        }

        public override Targets SelectTargets()
        {
            Targets target = new Targets();
            if (entityEnmity.Count() == 0)
                return target;

            // ai logic here
            // get weight intervals
            int[] intervals = new int[entityEnmity.Count()];
            // 1 5 1 1
            // 1 6 7 8
            intervals[0] = entityEnmity.ElementAt(0).Value;

            for (int index = 1; index < entityEnmity.Count; index++)
            {
                intervals[index] = intervals[index-1] + entityEnmity.ElementAt(index).Value;
            }

            int n = Rand(0, intervals[entityEnmity.Count() - 1] + 1);
            int intervalIndex = 0;

            while (n > intervals[intervalIndex++]) { }

            target.PrimaryTarget = entityEnmity.ElementAt(intervalIndex).Key;
            return target;
        }
        // TODO: clean out dictionary of dead entity
    }
}