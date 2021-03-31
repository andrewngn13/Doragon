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
        private IBattleEntity selfBattleEntity;

        // call pseudo constructor after monobehavior instantiation
        public void BattleTargettingSpriteMono(IBattleEntity self)
        {
            selfBattleEntity = self;
        }

        // send message upwards to disable highlighting on others
        private void OnMouseDown()
        {
            DLogger.Log(ZString.Format("{0} sprite clicked", selfBattleEntity.Name));
        }
    }
}