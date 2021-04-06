using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Doragon.Battle
{
    public class Targets
    {
        public IBattleEntity PrimaryTarget;
        public List<IBattleEntity> AdditionalTargets;
    }
    public class TargettingSystem : MonoBehaviour
    {
        [SerializeField] private Button confirmOK;
        [SerializeField] private Button cancel;
        public Targets targetBuffer = new Targets();
        public bool targettingOpen = false;
        private void Start()
        {
            confirmOK.gameObject.SetActive(false);
            cancel.gameObject.SetActive(false);
        }
        // TODO: figure out some data structure for group positioning of ice pierce and fire splash
        /// <summary>
        /// Sets available targetting list for auto targetting to any found BattleTargettingSprites
        /// </summary>
        /// <returns></returns>
        public List<BattleTargettingSprite> GetAvailableTargets()
        {
            return GameObject.FindObjectsOfType<BattleTargettingSprite>().ToList();
        }

        public bool IsDead(IBattleEntity target)
        {
            if (GetAvailableTargets().Where(tsprite => tsprite.selfBattleEntity == target).Count() == 0)
                return true;
            return false;
        }
        /// <summary>
        /// Attached to the confirm and cancel button. Returns the targets to be attacked from targetBuffer. Null if cancelled.
        /// </summary>
        /// <returns></returns>
        // TODO: restrict the available trgets according to targettingType
        public async UniTask<Targets> ConfirmUserTargets(ActionRole combatLine, TargettingType targetType)
        {
            var availTargets = GetAvailableTargets();
            availTargets.ForEach(t => t.EnableHpBar(true));
            SelectAvailableTarget(targetType, true);
            confirmOK.gameObject.SetActive(true);
            cancel.gameObject.SetActive(true);
            int cancelorConfirm = await UniTask.WhenAny(cancel.OnClickAsync(), confirmOK.OnClickAsync());
            confirmOK.gameObject.SetActive(false);
            cancel.gameObject.SetActive(false);
            targettingOpen = false;
            availTargets.ForEach(t =>
            {
                t.EnableHpBar(false);
                t.Highlight(false);
            });
            if (cancelorConfirm == 0)
                return null;
            return targetBuffer;
        }
        public Targets GetFinalTarget()
        {
            Targets finalTarget = targetBuffer;
            targetBuffer = new Targets();
            return finalTarget;
        }
        /// <summary>
        /// Finds an available target according to targetType. Sets the Targets targetBuffer to this target(s).
        /// </summary>
        /// <param name="targetType"></param>
        public Targets SelectAvailableTarget(TargettingType targetType, bool openTargets)
        {
            var availTargets = GetAvailableTargets();
            // TODO: what if there are no targets?
            // TODO: Get proper targettingType
            targettingOpen = openTargets;
            // TODO: skip switch if no availTarget
            switch (targetType)
            {
                case TargettingType.Single:
                    targetBuffer.PrimaryTarget = availTargets[0].OnMouseDown().selfBattleEntity;
                    break;
                    // row
                    // pierce
                    // splash
                    // all
            }
            return targetBuffer;
        }
        /// <summary>
        /// Fired from OnClick of <see cref="BattleTargettingSprite">. Sets the Targets targetBuffer to this target(s).
        /// </summary>
        /// <param name="target"></param>
        public void SetPrimaryTarget(BattleTargettingSprite target)
        {
            targetBuffer.PrimaryTarget = target.selfBattleEntity;
            // TODO: figure out query for additional targets
            // TODO: Add extra auto target selection for stab / electric typed aoe
            // use targettingType
            HighlightTargets(target);
        }
        /// <summary>
        /// Disables previous highlighting. Highlights primary and auxliary targets.
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="aux"></param>
        private void HighlightTargets(BattleTargettingSprite primary, List<BattleTargettingSprite> aux = null)
        {
            foreach (var t in GetAvailableTargets())
                t.Highlight(false);
            primary.Highlight(true);
            if (aux != null)
            {
                foreach (var ta in aux)
                    ta.Highlight(false);
            }
        }
    }
}