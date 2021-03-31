using System.Collections.Generic;
using System.Linq;
using Doragon.Logging;
using Cysharp.Text;

namespace Doragon.Battle
{
    public enum ActionRole
    {
        Primary, Auxiliary
    }
    public struct DamageRequest
    {
        ActionRole actionRole { get; }
        DamageType damageTyping { get; }
        ManaType manaTyping { get; }
        TargettingType targetTyping { get; }
        float damageMod { get; }
        int priority { get; }
        public IBattleEntity source { get; }
        public IBattleEntity target { get; set; }
        public DamageRequest(ActionRole role, DamageType damageType, ManaType manaType, TargettingType targetType, float damageModifier, int movePriority, IBattleEntity src, IBattleEntity tar)
        {
            actionRole = role;
            damageTyping = damageType;
            manaTyping = manaType;
            targetTyping = targetType;
            damageMod = damageModifier;
            priority = movePriority;
            source = src;
            target = tar;
        }
    }

    /// <summary>
    /// Select between the COMMAND or QUICKPLAY systems for determining how BattleEntity orders are determined.
    /// DEFAULT: COMMAND is a system that allows you to set the actions of all battle entities at the beginning of a round.
    /// Turn order will then be activated through a combination of an entities SPD stat, skill priority, and randomization.
    /// QUICKPLAY is a system that utilizes an action fill bar. Fill rate is determined by an entities SPD stat and randomization.
    /// In Quickplay, auto attacks are enabled, and entities can be queued if skills are not used in a timely fashion.
    /// </summary>
    public enum TurnType
    {
        COMMAND, QUICKPLAY
    }

    /// <summary>
    /// Process DamageRequest and execute accordingly
    /// </summary>
    public class DamageHandler
    {
        private static DamageHandler instance;
        private TurnType turnTyping;
        private static Stack<DamageRequest> damageRequests = new Stack<DamageRequest>();

        public DamageHandler(TurnType turnType = TurnType.COMMAND)
        {
            instance = this;
            turnTyping = turnType;
        }

        /// <summary>
        /// Command: Processes a damage request immediatedly and directs execution to animation.
        /// </summary>
        /// <param name="damageRequest"></param>
        /// <returns>A string representative of source execution towards target, or fumble.</returns>
        public string PushDamageRequest(DamageRequest damageRequest)
        {
            if (damageRequest.target == null)
            {
                DLogger.LogError("No target in this DamageRequest");
            }
            else
            {
                DLogger.Log(ZString.Format("{0} has pushed a DamageRequest to stack", damageRequest.source.Name));
            }
            // TODO: process a damageRequest
            // TODO: Defer animation and damageNumber data
            return ZString.Format("{0} fumbled!", damageRequest.source.Name);
        }
        /// <summary>
        /// Pops a damageRequest off the stack if possible. Called when backtracking the slayer collection.
        /// </summary>
        public void PopDamageRequest()
        {
            if (damageRequests.Count > 0)
                DLogger.Log(ZString.Format("{0}'s DamageRequest was popped off the stack", damageRequests.Pop().source.Name));
            else
                DLogger.LogError("DamageRequest stack is empty!");
        }

        /// <summary>
        /// Processes the stack of DamageRequests and execute in speed formula + skill priority order
        /// </summary>
        public void ProcessDamageRequests()
        {
            // sort by descending speed, damageRequest execution order
            var sortedRequests = damageRequests.ToList<DamageRequest>().OrderByDescending(s => s.source.GetSpeedRating());

        }
        /* TODO: immediateDamageRequest quickplay
        /// <summary>
        /// QUICKPLAY: Processes a damage request immediatedly and directs execution to animation.
        /// </summary>
        /// <param name="damageRequest"></param>
        /// <returns>A string representative of source execution towards target, or fumble.</returns>
        public string ImmediateDamageRequest(DamageRequest damageRequest)
        {
            if (damageRequest.target == null)
            {
                DLogger.LogError("No target in this damage request");
            }
            // TODO: process a damageRequest
            // TODO: Defer animation and damageNumber data
            return ZString.Format("{0} fumbled!", damageRequest.source.Name);
        }*/
    }
}
