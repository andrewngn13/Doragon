using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Doragon.Logging;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using TMPro;
using DG.Tweening;

namespace Doragon.Battle
{
    public class BattleUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject slayerProfilePrefab, lineDividerPrefab, actionMenu, roundConfirmPrompt, manaLevelPanel;
        [SerializeField] private Image slayerPortrait;
        [SerializeField] private Transform slayerLayout;
        [SerializeField] private Button normalAttackButton, backButton, roundConfirm, roundCancel;
        [SerializeField] private TextMeshProUGUI manaCalculations, manaSum;
        private DamageHandler damageHandler;
        private List<BattleSlayerProfile> slayerProfiles = new List<BattleSlayerProfile>();
        private int slayerIterator = 0;
        private const int relativeDistance = -50;
        private const float animateTime = 0.3f;

        private void Start()
        {
            roundConfirmPrompt.SetActive(false);
            ShowActionMenu(false);
        }

        /// <summary>
        /// Store the slayer collection for bidirectional traversal. Set the first slayer in the UI.
        /// Setup the enemy collection and targetting system.
        /// </summary>
        /// <param name="slayerCollection"></param>
        public async void BattleUIHandlerInit(List<IBattleEntity> battleEntityCollection)
        {
            damageHandler = new DamageHandler(battleEntityCollection, new ManaLevels(manaLevelPanel), manaCalculations, manaSum);
            // filled left to right, backline, line divider, frontline. slayerProfile list is filled upon finish
            slayerProfiles.AddRange(FillSlayerLine(battleEntityCollection.Where(s => s.MyTeam && !s.FrontLine)));
            Instantiate(lineDividerPrefab).transform.SetParent(slayerLayout, false);
            slayerProfiles.AddRange(FillSlayerLine(battleEntityCollection.Where(s => s.MyTeam && s.FrontLine)));

            SetSlayer(slayerIterator);
            normalAttackButton.onClick.AddListener(SetNormalAttackListener);
            backButton.onClick.AddListener(PrevSlayer);
            // wait 3 seconds
            await UniTask.Delay(3000);
            ShowActionMenu(true);
            slayerPortrait.gameObject.SetActive(false);
            await UniTask.CompletedTask;
        }
        /// <summary>
        /// Bind listener to Normal Attack button and await targetting. Pushes DamageRequest if target selected.
        /// </summary>
        private async void SetNormalAttackListener()
        {
            SetInteractable(actionMenu, false);
            DamageRequest request = slayerProfiles[slayerIterator].SelfBattleEntity.NormalAttack();
            Targets target = await damageHandler.ConfirmUserTargets(request.TargettingMyTeam, request.actionRole, request.TargetTyping);
            if (target.PrimaryTarget == null)
            {
                DLogger.Log("No target selected, cancelling targetting");
                SetInteractable(actionMenu, true);
            }
            else
            {
                request.target = target;
                DLogger.Log(ZString.Format("{0} selected for targetting", request.target.PrimaryTarget.Name));
                damageHandler.PushDamageRequest(request);
                NextSlayer();
                SetInteractable(actionMenu, true);
            }
        }
        /// <summary>
        /// Fills the <see cref="slayerLayout"> with <see cref="slayerProfilePrefab"> according to line. Returns List<BattleSlayerProfile>
        /// </summary>
        /// <param name="slayerLine"></param>
        private List<BattleSlayerProfile> FillSlayerLine(IEnumerable<IBattleEntity> slayerLine)
        {
            List<BattleSlayerProfile> addedProfiles = new List<BattleSlayerProfile>();
            slayerLine.OrderBy(o => o.LineIndex);
            var sb = ZString.CreateStringBuilder();
            sb.Append("Instantiating slayer profile of ");
            foreach (var slayer in slayerLine)
            {
                sb.Append(ZString.Format("{0}, ", slayer.Name));
                var slayerPrefab = Instantiate(slayerProfilePrefab);
                slayerPrefab.transform.SetParent(slayerLayout, false);
                var slayerObj = slayerPrefab.GetComponent<BattleSlayerProfile>();
                // binds the slayer data
                slayerObj.SelfBattleEntity = slayer;
                addedProfiles.Add(slayerObj);
            }
            DLogger.Log(sb.ToString().Substring(0, sb.Length - 2));
            sb.Dispose();
            return addedProfiles;
        }

        private async void SetSlayer(int slayerIndex)
        {
            if (slayerIterator == 0)
                backButton.interactable = false;
            // TODO: check if this slayer is dead, skip if so

            // TODO: make this a highlight instead
            slayerProfiles[slayerIndex].SetSelectableInteract(true);
            // TODO: make a pooling solution of the skills and portrait with disable/ enable instead of loading
            // TODO: UI: method to load up the skill ui with the battle entities skills
            // TODO: UI: Handle skill instantiation based on cast properties
            // TODO: UI: Handle skill instantiation to UI
            // TODO: UI: Handle skill targetting selection
            // TODO: check action role, do not set Primary skills if Auxiliary
            await AnimatedFadeInOutLeft(false);

            // TODO: set portrait
            // await slayerPortrait.sprite =    Resources.LoadAsync  slayerProfiles[slayerIndex].SelfBattleEntity.Name
            await AnimatedFadeInOutLeft(true);
        }
        private void NextSlayer()
        {
            if (slayerIterator < slayerProfiles.Count - 1)
            {
                // darken previous slayer
                slayerProfiles[slayerIterator].SetSelectableInteract(false);
                SetSlayer(++slayerIterator);
                DLogger.Log(ZString.Format("{0} is the current Slayer", slayerProfiles[slayerIterator].SelfBattleEntity.Name));
                backButton.interactable = true;
            }
            // TODO: make confirm prompt style better
            else if (slayerIterator == slayerProfiles.Count - 1)
            {
                slayerProfiles[slayerIterator].SetSelectableInteract(false);
                DLogger.Log("Requesting total DamageRequest confirmation");
                CancelOrConfirmRound();
            }
        }
        /// <summary>
        /// Iterates to previous BattleSlayerProfile if available.
        /// </summary>
        private void PrevSlayer()
        {
            if (slayerIterator > 0)
            {
                damageHandler.PopDamageRequest();
                slayerProfiles[slayerIterator].SetSelectableInteract(false);
                SetSlayer(--slayerIterator);
                DLogger.Log(ZString.Format("{0} is the current Slayer", slayerProfiles[slayerIterator].SelfBattleEntity.Name));
            }
        }

        private async void CancelOrConfirmRound()
        {
            ShowActionMenu(false);
            roundConfirmPrompt.SetActive(true);
            int cancelorConfirm = await UniTask.WhenAny(roundCancel.OnClickAsync(), roundConfirm.OnClickAsync());
            roundConfirmPrompt.SetActive(false);
            if (cancelorConfirm == 0)
            {
                damageHandler.PopDamageRequest();
                slayerProfiles[slayerIterator].SetSelectableInteract(true);
                ShowActionMenu(true);
                DLogger.Log("Round confirmation cancelled");
            }
            else if (cancelorConfirm == 1)
            {
                DLogger.Log("Round confirmation finished");
                manaCalculations.transform.parent.gameObject.SetActive(false);
                manaSum.transform.parent.gameObject.SetActive(false);
                manaCalculations.SetText("");
                manaSum.SetText("");
                await damageHandler.ProcessDamageRequests();
                slayerIterator = 0;
                SetSlayer(slayerIterator);
                SetInteractable(slayerLayout.gameObject, true);
                ShowActionMenu(true);
                manaCalculations.transform.parent.gameObject.SetActive(true);
                manaSum.transform.parent.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Hides or shows the Slayer menu
        /// </summary>
        /// <param name="showMenu"></param>
        private void ShowActionMenu(bool showMenu)
        {
            var task = AnimatedFadeInOutLeft(showMenu);
            slayerLayout.gameObject.SetActive(showMenu);
            actionMenu.SetActive(showMenu);
        }

        private async UniTask AnimatedFadeInOutLeft(bool showImage, float duration = animateTime, int relDist = relativeDistance)
        {
            var awaitFalse = slayerPortrait.transform.DOMoveX(showImage ? 0 : relDist, duration);
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration));
            // TODO: do i need slayer portrait? => interferes with battle sprites
            // await slayerPortrait.DOFade(showImage ? 1 : 0, duration);
        }

        /// <summary> 
        /// Finds all the Selectables in gameObject and sets the interactable to bool param enable
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="enable"></param>
        private void SetInteractable(GameObject gameObject, bool enable)
        {
            gameObject.GetComponentsInChildren<Selectable>().ToList().ForEach(s => s.interactable = enable);
        }
    }
}