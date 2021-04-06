using UnityEngine;
using Doragon.Logging;
using Cysharp.Text;

namespace Doragon.Battle
{
    [RequireComponent(typeof(Sprite))]
    [RequireComponent(typeof(Collider))]
    public class BattleTargettingSprite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer hpBarHighlight;
        [SerializeField] private SpriteRenderer hpBar;
        public IBattleEntity selfBattleEntity { get; set; }

        // call pseudo constructor after monobehavior instantiation
        public void BattleTargettingSpriteInit(IBattleEntity self)
        {
            selfBattleEntity = self;
            hpBarHighlight.enabled = false;
            hpBar.gameObject.SetActive(false);
        }

        // send message upwards to disable highlighting on others
        // only set target when targetting is open
        public BattleTargettingSprite OnMouseDown()
        {
            DLogger.Log(ZString.Format("{0} sprite clicked", selfBattleEntity.Name));
            var targetSys = GameObject.FindObjectOfType<TargettingSystem>();
            if (targetSys.targettingOpen)
                targetSys.SetPrimaryTarget(this);
            return this;
        }
        public void EnableHpBar(bool enable)
        {
            hpBar.gameObject.SetActive(enable);
        }
        public void Highlight(bool enable)
        {
            hpBarHighlight.enabled = enable;
        }
    }
}