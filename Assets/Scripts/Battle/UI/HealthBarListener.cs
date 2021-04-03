using System;
using UnityEngine.UI;
using UnityEngine;

namespace Doragon.Battle
{
    /// <summary>
    /// Change hp bar when health change detected on IBattleEntity
    /// </summary>
    /// TODO: hp bar listener
    public class HealthBarListener
    {
        public HealthBarListener(IBattleEntity battleEntity, Image hpBar)
        {
            
        }
        public HealthBarListener(IBattleEntity battleEntity, SpriteRenderer hpBar)
        {

        }

        private void TweenHPBar(Image hpBar)
        {
            
        }
    }
}