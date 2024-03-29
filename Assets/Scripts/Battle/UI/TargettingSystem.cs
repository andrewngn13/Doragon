using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Text;
using DG.Tweening;
using Doragon.Logging;
namespace Doragon.Battle
{
    public struct Targets
    {
        public IBattleEntity PrimaryTarget;
        public List<IBattleEntity> AdditionalTargets;
    }
    public class TargettingSystem : MonoBehaviour
    {
        [SerializeField] private GameObject genericBattleSprite;
        [SerializeField] private Button confirmOK, cancel;
        public Targets targetBuffer = new Targets();
        public bool CanHighlight = false;
        private const int OffscreenOffset = 15;
        private const float animateTime = 2.4f;
        private void Start()
        {
            confirmOK.gameObject.SetActive(false);
            cancel.gameObject.SetActive(false);
        }
        // TODO: figure out some data structure for group positioning of ice pierce and fire splash
        /// <summary>
        /// Returns a list of any BattleTargettingSprite found on Unity objects in scene
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
        public async UniTask<Targets> ConfirmUserTargets(bool myTeam, ActionRole actionRole, TargettingType targetType)
        {
            var availTargets = GetAvailableTargets();
            availTargets.ForEach(t => t.EnableHpBar(true));
            SelectAvailableTarget(myTeam, actionRole, targetType, true);
            confirmOK.gameObject.SetActive(true);
            cancel.gameObject.SetActive(true);
            int cancelorConfirm = await UniTask.WhenAny(cancel.OnClickAsync(), confirmOK.OnClickAsync());
            confirmOK.gameObject.SetActive(false);
            cancel.gameObject.SetActive(false);
            availTargets.ForEach(t =>
            {
                t.canTarget = false;
                t.EnableHpBar(false);
                t.Highlight(false);
            });
            if (cancelorConfirm == 0)
                return new Targets();
            return targetBuffer;
        }

        /// <summary>
        /// Finds an available target according to targetType. Sets the Targets targetBuffer to this target(s).
        /// </summary>
        /// <param name="targetType"></param>
        public Targets SelectAvailableTarget(bool myTeam, ActionRole actionRole, TargettingType targetType, bool openTargets)
        {
            var availTargets = GetAvailableTargets().Where(t => t.selfBattleEntity.MyTeam == myTeam).ToList().
                OrderBy(s => !s.selfBattleEntity.FrontLine).ThenBy(t => t.selfBattleEntity.LineIndex).ToList();
            // TODO: what if there are no targets?
            // TODO: Get proper targettingType
            // TODO: fix actionRole targetting
            availTargets.ForEach(t => t.canTarget = true);

            if (availTargets.Count() == 0)
                return new Targets();
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
            // dont highlight unless targetting on
            if (!CanHighlight) return;
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

        public void SpawnBattleSprites(ICollection<IBattleEntity> battleEntityCollection)
        {
            // TODO: create battle sprites of slayers and enemies
            // TODO: spawn offscreen and run in animation
            List<BattleTargettingSprite> spriteTransforms = new List<BattleTargettingSprite>();
            var sb = ZString.CreateStringBuilder();
            sb.Append("Instantiating battle sprites of ");
            if (battleEntityCollection.Count == 0)
                return;
            foreach (var entity in battleEntityCollection)
            {
                BattleTargettingSprite sprite = Instantiate(genericBattleSprite).GetComponent<BattleTargettingSprite>();
                sprite.BattleTargettingSpriteInit(entity);
                sprite.GetComponentInChildren<SpriteRenderer>().sortingOrder = sprite.selfBattleEntity.LineIndex;
                spriteTransforms.Add(sprite);

                // TODO: sprite screen spawning location
                if (entity.MyTeam)
                {
                    sprite.transform.position = new Vector3(
                        -OffscreenOffset - entity.LineIndex - (!entity.FrontLine ? 4 : 0),
                         1 - entity.LineIndex, 0);
                }
                else // enemy team
                {
                    sprite.transform.position = new Vector3(
                        OffscreenOffset + entity.LineIndex + (!entity.FrontLine ? 4 : 0),
                         1 - entity.LineIndex, 0);
                    sprite.sprite.flipX = true;
                }
                sb.Append(ZString.Format("{0}, ", entity.Name));
            }
            spriteTransforms.ForEach(t =>
            {
                if (t.selfBattleEntity.MyTeam)
                    t.transform.DOMoveX(t.transform.position.x + OffscreenOffset * 0.9f, animateTime);
                else
                    t.transform.DOMoveX(t.transform.position.x - OffscreenOffset * 0.9f, animateTime);
            });
            DLogger.Log(sb.ToString().Substring(0, sb.Length - 2));
            sb.Dispose();
        }
    }
}