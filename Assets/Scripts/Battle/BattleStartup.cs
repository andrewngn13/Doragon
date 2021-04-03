using UnityEngine;
using System.Collections.Generic;
using Doragon.Logging;

namespace Doragon.Battle
{
    public class BattleStartup
    {
        // TODO: Get startup cast members of battle injected to this class with proper parameters
        /// <summary>
        /// A debug constructor, assume battle scene is open
        /// </summary>
        public BattleStartup(string debug)
        {
            List<IBattleEntity> battleEntities = new List<IBattleEntity>();
            BattleEntityFactory factory = new BattleEntityFactory();
            // TODO: Make a data validator instead of sanitizing in all my methods
            DLogger.Log("Producing a frontline VAN battle entity.");
            battleEntities.Add(factory.Build("VAN", "Nan", true, true, 0));
            DLogger.Log("Producing a frontline VAN battle entity.");
            battleEntities.Add(factory.Build("VAN", "Kazan", true, true, 1));
            DLogger.Log("Producing a frontline VAN battle entity.");
            battleEntities.Add(factory.Build("VAN", "Woolie", true, true, 2));
            DLogger.Log("Producing a backline VAN battle entity.");
            battleEntities.Add(factory.Build("VAN", "Ada", true, false, 0));

            // TODO: Instantiate models
            var handler = GameObject.FindObjectOfType<BattleUIHandler>();
            DLogger.Log("Setting up UI components: Slayer profiles, Slayer sprites, enemy sprites");
            DLogger.LogWarning("TODO UI components: Skill viewer");
            handler.BattleUIHandlerInit(battleEntities);
            // TODO: UI: method to load up the skill ui with the battle entities skills
            // TODO: UI: Handle skill instantiation based on cast properties
            // TODO: UI: Handle skill instantiation to UI
            // TODO: UI: Handle skill targetting selection
            // TODO: start a laptimer
        }
        // TODO: an actual production worthy constructor for battle startup
    }
}