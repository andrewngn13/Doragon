using UnityEngine;
using Doragon.Logging;
using Cysharp.Text;
using DG.Tweening;
using Cysharp.Threading.Tasks;
namespace Doragon.Battle
{
    [RequireComponent(typeof(Sprite))]
    [RequireComponent(typeof(Collider))]
    public class BattleTargettingSprite : MonoBehaviour
    {
        [SerializeField] public SpriteRenderer sprite;
        [SerializeField] private SpriteRenderer hpBarHighlight, hpBar, currentHpBar;
        public IBattleEntity selfBattleEntity { get; set; }
        public bool canTarget = false;
        private int cachedHP;
        private const float tweenTime = 0.5f;

        // call pseudo constructor after monobehavior instantiation
        public void BattleTargettingSpriteInit(IBattleEntity self)
        {
            selfBattleEntity = self;
            hpBarHighlight.enabled = false;
            hpBar.gameObject.SetActive(false);
            cachedHP = self.CurrentHP;
        }

        // send message upwards to disable highlighting on others
        // only set target when targetting is open
        public BattleTargettingSprite OnMouseDown()
        {
            DLogger.Log(ZString.Format("{0} sprite clicked", selfBattleEntity.Name));
            var targetSys = GameObject.FindObjectOfType<TargettingSystem>();
            if (canTarget)
                targetSys.SetPrimaryTarget(this);
            return this;
        }
        public void EnableHpBar(bool enable)
        {
            // always show hp bar if damaged
            if (selfBattleEntity.CurrentHP < selfBattleEntity.HP)
            {
                hpBar.gameObject.SetActive(true);
                return;
            }
            hpBar.gameObject.SetActive(enable);
        }
        public void Highlight(bool enable)
        {
            hpBarHighlight.enabled = enable;
        }

        public async UniTask DeathFade()
        {
            hpBarHighlight.DOFade(0,tweenTime);
            hpBar.DOFade(0,tweenTime);
            currentHpBar.DOFade(0,tweenTime);
            await sprite.DOFade(0,tweenTime);
        }

        // TODO: move this code out of Update() to increase performance
        private void Update()
        {
            if (selfBattleEntity.CurrentHP == cachedHP) return;
            cachedHP = selfBattleEntity.CurrentHP;
            EnableHpBar(true);
            currentHpBar.transform.DOScaleX((float)cachedHP / selfBattleEntity.HP, tweenTime);
        }
    }
}