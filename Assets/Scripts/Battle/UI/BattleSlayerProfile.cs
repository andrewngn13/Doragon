using UnityEngine;
using UnityEngine.UI;
using Doragon.Logging;
using Cysharp.Text;

namespace Doragon.Battle
{
    /// <summary>
    /// Instantiated Monobehavior that requires at minimum a health bar reference and self IBattleEntity
    /// </summary>
    public class BattleSlayerProfile : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        // only available on slayer profile prefabs
        [SerializeField] private Selectable profileImage;
        public IBattleEntity SelfBattleEntity { get; set; }
        private void Awake()
        {
            if (healthBar == null)
                DLogger.LogError(ZString.Format("Health bar of {0} null!", this.name));
            if (profileImage == null)
                DLogger.LogWarning(ZString.Format("Selectable of {0} null. Is this an enemy?", this.name));
        }
        public void SetSelectableInteract(bool enable)
        {
            if (profileImage == null)
                DLogger.LogError(ZString.Format("Selectable of {0} null! Selectable setting failed.", this.name));
            profileImage.interactable = enable;
        }
    }
}