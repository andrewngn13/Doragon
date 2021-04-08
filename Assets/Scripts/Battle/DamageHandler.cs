using System;
using System.Collections.Generic;
using System.Linq;
using Doragon.Logging;
using Cysharp.Text;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
namespace Doragon.Battle
{
    public enum ActionRole
    {
        Primary, Auxiliary
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
        private const int minSpeedMod = 90, maxSpeedMod = 130;
        private ManaLevels manaLevel;
        private TurnType turnTyping;
        private TextMeshProUGUI manaCalc, manaSum;
        private readonly string[] colors = { "red", "green", "blue", "purple" };
        private Stack<DamageRequest> damageRequests = new Stack<DamageRequest>();
        private TargettingSystem targetSystem;
        private System.Random rand = new System.Random();

        public DamageHandler(List<IBattleEntity> battleEntityCollection, ManaLevels manaLevels, TextMeshProUGUI manaCalcText, TextMeshProUGUI manaSumText, TurnType turnType = TurnType.COMMAND)
        {
            manaLevel = manaLevels;
            manaCalc = manaCalcText;
            manaSum = manaSumText;
            turnTyping = turnType;
            targetSystem = GameObject.FindObjectOfType<TargettingSystem>();
            targetSystem.SpawnBattleSprites(battleEntityCollection);
        }
        /// <summary>
        /// Command: Processes a damage request and pushes it to the stack if target field is defined. Updates mana calc texts.
        /// </summary>
        /// <param name="damageRequest"></param>
        public void PushDamageRequest(DamageRequest damageRequest)
        {
            if (damageRequest.target.PrimaryTarget == null)
            {
                throw new System.ArgumentNullException("No target in this DamageRequest");
            }
            else if (damageRequest.source == null)
            {
                throw new System.ArgumentNullException("No source in this DamageRequest. Wait, but how?");
            }
            else
            {
                damageRequests.Push(damageRequest);
                UpdateColorCalc(damageRequests, manaCalc, manaSum);
                damageRequests.ToList().ForEach(s =>
                    DLogger.Log(ZString.Format("{0}:{1}, ", s.source.Name, s.target.PrimaryTarget.Name)));
                DLogger.Log(ZString.Format("{0} has pushed a DamageRequest to stack. {1} requests in stack.", damageRequest.source.Name, damageRequests.Count));
            }
        }
#if UNITY_EDITOR
        public Stack<DamageRequest> GetDamageRequests()
        {
            return damageRequests;
        }
#endif
        /// <summary>
        /// Pops a damageRequest off the stack if possible. Called when backtracking the slayer collection. Updates Mana Color text.
        /// </summary>
        public void PopDamageRequest()
        {
            if (damageRequests.Count > 0)
            {
                var popped = damageRequests.Pop();
                UpdateColorCalc(damageRequests, manaCalc, manaSum);
                DLogger.Log(ZString.Format("{0}'s DamageRequest was popped off the stack. {1} requests in stack.", popped.source.Name, damageRequests.Count));
            }
            else
            {
                DLogger.LogWarning("Stack is empty or out of bounds");
                throw new System.IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Processes the stack of DamageRequests and execute in speed formula + skill priority order
        /// stack must be filled using <see cref="PushDamageRequest()">
        /// </summary>
        // TODO: Execute damage requests
        public async UniTask ProcessDamageRequests()
        {
            DLogger.Log("Starting DamageRequest stack processing");
            if (damageRequests.Count <= 0)
                throw new System.IndexOutOfRangeException("DamageRequest stack is empty or out of bounds");
            // TODO: Modularize speed calculations for easier testing / tweaking
            // we create a new sorted by speed list of requests
            var sb = ZString.CreateStringBuilder();
            var sortedRequests = damageRequests.ToList<DamageRequest>().OrderByDescending(s => s.source.SPD * rand.Next(minSpeedMod, maxSpeedMod));
            sortedRequests.ToList().ForEach(s => sb.AppendFormat("{0}: {1}, ", s.source.Name, s.source.SPD * rand.Next(minSpeedMod, maxSpeedMod)));
            sb.Append("\nRound Order: ");
            sortedRequests.ToList().ForEach(s => sb.AppendFormat("{0}:{1}, ", s.source.Name, s.target.PrimaryTarget.Name));
            DLogger.Log(sb.ToString());
            sb.Dispose();
            //

            foreach (var r in sortedRequests)
            {
                Targets targetWrapper = r.target;
                // TODO: Add auto target selection when main target cannot be selected
                DLogger.LogWarning("Fake animation sequence of 3 seconds");

                await UniTask.Delay(TimeSpan.FromSeconds(3));
                if (targetSystem.IsDead(targetWrapper.PrimaryTarget))
                {
                    DLogger.LogWarning("IsDead triggered, replacing target!");
                    // replace the target! if we cant replace the target, fumble!
                    var newTarget = targetSystem.SelectAvailableTarget(r.TargettingMyTeam, r.actionRole, r.TargetTyping, false);
                    if (newTarget.PrimaryTarget == null)
                    {
                        DLogger.Log(ZString.Format("{0} fumbled with no target!", r.source.Name));
                        continue;
                    }
                    else
                    {
                        targetWrapper = newTarget;
                    }
                }
                // TODO: fumbled if not enough mana
                if (!manaLevel.AddMana(r.ManaChange))
                {
                    DLogger.Log(ZString.Format("{0} fumbled from no mana!", r.source.Name));
                    // TODO: fumble animation?
                    continue;
                }
                // TODO: attach damage to a projectile or animation hit
                // TODO: animate the hp bar
                targetWrapper.PrimaryTarget.HP -= 5;
                manaLevel.AnimateMana();
                // TODO: output action to the log
                // TODO: more involved damage, buffing, and debuffing
                // TODO: damage formula

                // TODO: deathchecking
                if (targetWrapper.PrimaryTarget.HP <= 0)
                {
                    UnityEngine.GameObject.Destroy(targetSystem.GetAvailableTargets().Single(tsprite => tsprite.selfBattleEntity == targetWrapper.PrimaryTarget).gameObject);
                    await UniTask.NextFrame();
                    DLogger.Log(ZString.Format("{0} has been destroyed", targetWrapper.PrimaryTarget.Name));
                    // TODO: Win condition
                    if (targetSystem.GetAvailableTargets().Where(t => !t.selfBattleEntity.MyTeam).Count() == 0)
                    {
                        DLogger.Log("The fight is won");
                        break;
                    }
                }
                else
                    DLogger.Log(ZString.Format("{0} has {1} HP now", targetWrapper.PrimaryTarget.Name, r.target.PrimaryTarget.HP));
                // more details about death: must darken a dead slayer frame, must death animate target sprites, 
                // remove any damageRequests with them as the source
                // must update BattleUIHandlers collection of Battle profiles so dead ones are skipped
                // reset targetting system available data with GetAvailableTargets
                // ??? can we revive. sounds complicated.
                // would have to not dispose of slayer targetting sprites / myTeam property
            }
            // clear the stack
            damageRequests.Clear();
            // TODO: Defer animation and damageNumber data
            DLogger.Log("DamageRequest stack processing complete");
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// Sets manaCalcText as DamageRequest mana changes per line, with delta mana change text on manaSumText
        /// </summary>
        /// <param name="damageRequestStack"></param>
        /// <param name="manaCalcText"></param>
        /// <param name="manaSumText"></param>
        private void UpdateColorCalc(Stack<DamageRequest> damageRequestStack, TextMeshProUGUI manaCalcText, TextMeshProUGUI manaSumText)
        {
            using (var sb = ZString.CreateStringBuilder())
            {
                int[] deltaMana = new int[4];
                damageRequestStack.ToList().ForEach(dr =>
                {
                    // format manaChange into string builder
                    for (int i = 0; i < dr.ManaChange.Length; i++)
                    {
                        deltaMana[i] += dr.ManaChange[i];
                        if (dr.ManaChange[i] >= 0)
                            sb.AppendFormat("<color=\"{1}\">+{0}</color> ", dr.ManaChange[i], colors[i]);
                        else
                            sb.AppendFormat("<color=\"{1}\">{0}</color> ", dr.ManaChange[i], colors[i]);
                    }
                    sb.Append("\n");
                });
                manaCalcText.SetText(sb.ToString());
                // clear buffer, format delta mana
                sb.Clear();
                for (int i = 0; i < deltaMana.Length; i++)
                {
                    if (deltaMana[i] >= 0)
                        sb.AppendFormat("<color=\"{1}\">+{0}</color> ", deltaMana[i], colors[i]);
                    else
                        sb.AppendFormat("<color=\"{1}\">{0}</color> ", deltaMana[i], colors[i]);
                }
                manaSumText.SetText(sb.ToString());
            }
        }

        public async UniTask<Targets> ConfirmUserTargets(bool myTeam, ActionRole actionRole, TargettingType targetType)
        {
            targetSystem.CanHighlight = true;
            var target = await targetSystem.ConfirmUserTargets(myTeam, actionRole, targetType);
            targetSystem.CanHighlight = false;
            return target;
        }

        /* TODO: immediateDamageRequest
        /// <summary>
        /// Processes a damage request immediatedly and directs execution to animation.
        /// </summary>
        /// <param name="damageRequest"></param>
        /// <returns>A string representative of source execution towards target, or fumble.</returns>
        public string ImmediateDamageRequest(DamageRequest damageRequest)
        {
        }*/
    }
}
