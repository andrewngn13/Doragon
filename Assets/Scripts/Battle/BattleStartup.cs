using UnityEngine;
using System.Collections.Generic;
using Doragon.Logging;

namespace Doragon.Battle
{
    public class BattleStartup
    {
#if UNITY_EDITOR

        // TODO: Get startup cast members of battle injected to this class with proper parameters
        /// <summary>
        /// A debug constructor, assume battle scene is open
        /// </summary>
        public BattleStartup()
        {
            List<IBattleEntity> battleEntities = new List<IBattleEntity>();
            BattleEntityFactory factory = new BattleEntityFactory();
            /* 
            for (int i = 0; i < 3; i++)
            {
                DLogger.Log("Producing a frontline VAN battle entity.");
                var product = factory.Build("VAN", true, true);
                if (product == null)
                {
                    DLogger.LogError("Factory failed to produce battle entity.");
                    return;
                }
                battleEntities.Add(product);
            }
            */
            // making a backline VAN
            DLogger.Log("Producing a frontline VAN battle entity.");
            battleEntities.Add(factory.Build("VAN", "Nan", true, true));
            DLogger.Log("Producing a frontline VAN battle entity.");
            battleEntities.Add(factory.Build("VAN", "Kazan", true, true));
            DLogger.Log("Producing a frontline VAN battle entity.");
            battleEntities.Add(factory.Build("VAN", "Woolie", true, true));
            DLogger.Log("Producing a backline VAN battle entity.");
            battleEntities.Add(factory.Build("VAN", "Ada", true, false));

            // bind the IBattleEntity to the instantiated sprite model and add a get
            // TODO: Instantiate models
            
            // TODO: Fill monobehavior input handler with slayers
            var handler = GameObject.FindObjectOfType<BattleUIHandler>();
            handler.SetDamageHandler(new DamageHandler());
            DLogger.Log("Setting up Slayer profiles");
            handler.SetUpSlayers(battleEntities);
            // TODO: UI: method to load up the skill ui with the battle entities skills
        }
#endif
        // TODO: UI: Handle skill instantiation based on cast properties
        // TODO: UI: Handle skill instantiation to UI
        // TODO: UI: Handle skill targetting selection
        // TODO: Send damage request up to be stored, have a damage request collection
        // TODO: UI: Pop requests off the stack if backpedaled in UI
        // TODO: Damage requests are their own type, and can be stored generically
        // TODO: Execute damage requests
        // TODO: Add auto target selection when main target cannot be selected
        // TODO: Add extra auto target selection for stab / electric typed aoe
        // TODO: Formulate the damage formula

        // TODO: an actual production worthy constructor for battle startup
    }
}