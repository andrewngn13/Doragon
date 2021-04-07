using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Doragon.Logging;
using Doragon.Extensions;

namespace Doragon.Battle
{
    public class BattleStartup
    {
        // TODO: Get startup cast members of battle injected to this class with proper parameters
        /// <summary>
        /// A debug constructor. Construct inside the Battle scene.
        /// </summary>
        public BattleStartup(string debug)
        {
            List<IBattleEntity> battleEntities = new List<IBattleEntity>();
            BattleEntityFactory factory = new BattleEntityFactory();
            DLogger.Log("Producing 3 player frontline VAN {Nan, Kazan, Woolie} and 1 backline VAN {ADA}");
            battleEntities.Add(factory.Build("VAN", "Nan", true, true, 0));
            battleEntities.Add(factory.Build("VAN", "Kazan", true, true, 1));
            battleEntities.Add(factory.Build("VAN", "Woolie", true, true, 2));
            battleEntities.Add(factory.Build("VAN", "Ada", true, false, 0));
            DLogger.Log("Producing 1 enemy frontline VAN {Doragon}");
            battleEntities.Add(factory.Build("VAN", "Doragon", false, true, 0));

            if (!ValidateTeam(battleEntities.Where(e => e.MyTeam)))
                throw new System.ArgumentException("My team battle entities are not valid for use");
            if (!ValidateTeam(battleEntities.Where(e => !e.MyTeam)))
                throw new System.ArgumentException("Enemy battle entities are not valid for use");
            DLogger.Log("Entities validated");
            var handler = GameObject.FindObjectOfType<BattleUIHandler>();
            DLogger.Log("Setting up UI components: Slayer profiles, Slayer sprites, enemy sprites");
            DLogger.LogWarning("TODO UI components: Skill viewer");
            handler.BattleUIHandlerInit(battleEntities);
            // TODO: start a laptimer
        }
        // TODO: an actual production worthy constructor for battle startup
        public BattleStartup()
        {
            // if (ValidateEntities() == true)
            //if (!ValidateTeam(battleEntities.Where(e => e.MyTeam)))
            //    throw new System.ArgumentException("My team battle entities are not valid for use");
            //if (!ValidateTeam(battleEntities.Where(e => !e.MyTeam)))
            //    throw new System.ArgumentException("Enemy battle entities are not valid for use");
        }

        /// <summary>
        /// Returns a bool on whather a team fits battle parameters
        // at least 1 frontliner
        // maximum of 5 per team
        // maximum of 3 per frontline 
        // max of 3 per backline
        // check for lineIndex conflicts
        // check lineIndex in range of 0-2 inclusive
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool ValidateTeam(IEnumerable<IBattleEntity> entities)
        {
            int myCount = entities.Count();
            var frontliners = entities.Where(entities => entities.FrontLine);
            int fCount = frontliners.Count();
            var backliners = entities.Where(entities => !entities.FrontLine);
            int bCount = backliners.Count();
            if (myCount == 0 || myCount > 5)
            {
                DLogger.LogWarning("Team count is out of range");
                return false;
            }
            if (fCount == 0 || fCount > 3)
            {
                DLogger.LogWarning("Frontliner count is out of range");
                return false;
            }
            if (bCount > 3)
            {
                DLogger.LogWarning("Backliner count is out of range");
                return false;
            }
            if (frontliners.DistinctBy(e => e.LineIndex).Count() != fCount ||
            frontliners.Where(e => e.LineIndex >= 0 && e.LineIndex < 3).Count() != fCount)
            {
                DLogger.LogWarning("Frontliner lineIndex is not distinct or is out of range");
                return false;
            }
            if (backliners.DistinctBy(e => e.LineIndex).Count() != bCount ||
            backliners.Where(e => e.LineIndex >= 0 && e.LineIndex < 3).Count() != bCount)
            {
                DLogger.LogWarning("Backliner lineIndex is not distinct or is out of range");
                return false;
            }
            return true;
        }
    }
}