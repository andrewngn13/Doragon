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
        private static List<BattleTargettingSprite> availTargets;
        private static Targets targetBuffer = new Targets();
        // TODO: restrict the available trgets according to targettingType
        private static TargettingType targettingType;
        public static bool targettingOpen = false;
        private void Start()
        {
            confirmOK.gameObject.SetActive(false);
            cancel.gameObject.SetActive(false);
        }
        // TODO: figure out some data structure for group positioning of ice pierce and fire splash
        public static List<BattleTargettingSprite> GetAvailableTargets()
        {
            var availTargetlist = GameObject.FindObjectsOfType<BattleTargettingSprite>().ToList();
            availTargets = availTargetlist;
            return availTargets;
        }
        /// <summary>
        /// Attached to the confirm and cancel button. Returns the targets to be attacked from targetBuffer. Null if cancelled.
        /// </summary>
        /// <returns></returns>
        public async UniTask<Targets> ConfirmUserTargets(ActionRole combatLine, TargettingType targetType)
        {
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
        /// <summary>
        /// Finds an available target according to targetType. Sets the Targets targetBuffer to this target(s).
        /// </summary>
        /// <param name="targetType"></param>
        private void SelectAvailableTarget(TargettingType targetType, bool openTargets)
        {
            // TODO: what if there are no targets?
            // TODO: Get proper targettingType
            targettingOpen = openTargets;
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
        }
        /// <summary>
        /// Fired from OnClick of <see cref="BattleTargettingSprite">. Sets the Targets targetBuffer to this target(s).
        /// </summary>
        /// <param name="target"></param>
        public static void SetPrimaryTarget(BattleTargettingSprite target)
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
        private static void HighlightTargets(BattleTargettingSprite primary, List<BattleTargettingSprite> aux = null)
        {
            foreach (var t in availTargets)
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