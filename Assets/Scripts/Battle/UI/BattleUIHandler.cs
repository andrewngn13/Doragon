using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Doragon.Logging;
using Cysharp.Text;

namespace Doragon.Battle
{
    public class BattleUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject slayerProfilePrefab;
        [SerializeField] private GameObject genericBattleSprite;
        [SerializeField] private GameObject lineDividerPrefab;
        [SerializeField] private Image slayerPortrait;
        [SerializeField] private Transform slayerLayout;
        [SerializeField] private Button normalAttackButton, backButton;
        [SerializeField] private ScrollRect skillScrollRect;
        private DamageHandler damageHandler;
        private readonly List<BattleSlayerProfile> slayerProfiles = new List<BattleSlayerProfile>();
        private int slayerIterator = 0;

        public void SetDamageHandler(DamageHandler handler)
        {
            damageHandler = handler;
        }

        /// <summary>
        /// Store the slayer collection for bidirectional traversal. Set the first slayer in the UI.
        /// Setup the enemy collection and targetting system.
        /// </summary>
        /// <param name="slayerCollection"></param>
        public void SetUpSlayers(List<IBattleEntity> battleEntityCollection)
        {
            // TODO: Run a test that checks that frontline exists, else force backline up to frontline
            // TODO: Run a test that checks maximum 3 frontline or 3 backline

            // filled left to right, backline, line divider, frontline. slayerProfile list is filled upon finish
            FillSlayerLine(battleEntityCollection.Where(s => s.MyTeam && !s.FrontLine));
            Instantiate(lineDividerPrefab).transform.SetParent(slayerLayout, false);
            FillSlayerLine(battleEntityCollection.Where(s => s.MyTeam && s.FrontLine));
            // darken all slayers
            foreach (var s in slayerProfiles)
                s.SetSelectableInteract(false);
            // set the first slayer
            SetSlayer(slayerProfiles[slayerIterator]);

            // TODO: create battle sprites of slayers and enemies
            // TODO: spacing of target sprites, or run in from offscreen
            List<BattleTargettingSprite> spriteList = new List<BattleTargettingSprite>();
            foreach (var entity in battleEntityCollection)
            {
                BattleTargettingSprite sprite = Instantiate(genericBattleSprite).GetComponent<BattleTargettingSprite>();
                sprite.BattleTargettingSpriteMono(entity);
                spriteList.Add(sprite);
            }
            // TODO: setup targetting
            // targettingsystem(spritelist)
        }

        /// <summary>
        /// Fills the <see cref="slayerLayout"> with <see cref="slayerProfilePrefab"> according to line.
        /// </summary>
        /// <param name="slayerLine"></param>
        /// <param name="frontline"></param>
        private void FillSlayerLine(IEnumerable<IBattleEntity> slayerLine)
        {
            foreach (var slayer in slayerLine)
            {
                DLogger.Log(ZString.Format("Instantiating slayer profile of {0}", slayer.Name));
                var slayerPrefab = Instantiate(slayerProfilePrefab);
                slayerPrefab.transform.SetParent(slayerLayout, false);
                var slayerObj = slayerPrefab.GetComponent<BattleSlayerProfile>();
                // binds the slayer data
                slayerObj.SelfBattleEntity = slayer;
                slayerProfiles.Add(slayerObj);
            }
        }

        private void SetSlayer(BattleSlayerProfile slayer)
        {
            slayer.SetSelectableInteract(true);
            // TODO: set slayer portrait
            // TODO: set slayer normal attack
            normalAttackButton.onClick.RemoveAllListeners();
            normalAttackButton.onClick.AddListener(() =>
           {
               damageHandler.PushDamageRequest(slayer.SelfBattleEntity.NormalAttack());
               NextSlayer();
           });
            // TODO: set slayer skills
        }
        // TODO: method to traverse the slayer collection back and forth
        private void NextSlayer()
        {
            if (slayerIterator < slayerProfiles.Count)
            {
                // TODO: darken previous slayer
                slayerProfiles[slayerIterator].SetSelectableInteract(false);
                SetSlayer(slayerProfiles[++slayerIterator]);
            }
            // TODO: if final slayer, change to execution button
            else if (slayerIterator == slayerProfiles.Count)
            {
                // change the ui interface to execution type ehre
            }

        }
        private void PrevSlayer()
        {
            // DamageHandler.PopDamageRequest
            // SetSlayer
            // TODO: darken next slayer
            // TODO: disable back button if first slayer
        }

    }
}