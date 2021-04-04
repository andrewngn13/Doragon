using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Doragon.Logging;
using Cysharp.Text;
namespace Doragon.Battle
{
    public class ManaLevels
    {
        private const float manaPanelWidth = 400f, minWidth = 35f, tweenTime = 0.5f;
        private int[] manaLevels = new int[4] { 25, 25, 25, 25 };
        private Image redBar, greenBar, blueBar, violetBar;
        private TextMeshProUGUI redText, greenText, blueText, violetText;
        // TODO: add abnormal starting mana, and abnormal adjustment
        public ManaLevels(GameObject manaPanel)
        {
            if (manaPanel == null)
            {
                DLogger.LogWarning("No manaPanel object, are we editor testing?");
                return;
            }
            var imageArray = manaPanel.GetComponentsInChildren<Image>();
            var tmproArray = manaPanel.GetComponentsInChildren<TextMeshProUGUI>();
            redBar = imageArray[0];
            redText = tmproArray[0];
            greenBar = imageArray[1];
            greenText = tmproArray[1];
            blueBar = imageArray[2];
            blueText = tmproArray[2];
            violetBar = imageArray[3];
            violetText = tmproArray[3];
        }

        /// <summary>
        /// Return true if mana is sucessfully added, else false. Must call AnimateMana after if GUI changes are wanted.
        /// </summary>
        public bool AddMana(int[] manaChange)
        {
            if (manaChange.Length != 4)
                throw new System.ArgumentOutOfRangeException("Invalid number of mana values");
            var newLevels = manaLevels.Zip(manaChange, (one, two) => one + two);
            if (newLevels.Where(m => m < 0).Count() > 0)
                return false;
            var sb = ZString.CreateStringBuilder();
            sb.Append("Mana change accepted, Mana levels are now ");
            manaLevels = newLevels.ToArray();
            manaLevels.ToList().ForEach(ml => sb.Append(ZString.Format("{0} ", ml)));
            DLogger.Log(sb.ToString());
            return true;
        }

        /// <summary>
        /// Tweens mana levels to stored class levels
        /// </summary>
        public void AnimateMana()
        {
            // TODO: tweening images
            int sum = manaLevels.Sum();
            redBar.rectTransform.DOSizeDelta(new Vector2(Mathf.Max(minWidth, (float)manaLevels[0] / sum * manaPanelWidth), redBar.rectTransform.sizeDelta.y), tweenTime);
            greenBar.rectTransform.DOSizeDelta(new Vector2(Mathf.Max(minWidth, (float)manaLevels[1] / sum * manaPanelWidth), greenBar.rectTransform.sizeDelta.y), tweenTime);
            blueBar.rectTransform.DOSizeDelta(new Vector2(Mathf.Max(minWidth, (float)manaLevels[2] / sum * manaPanelWidth), blueBar.rectTransform.sizeDelta.y), tweenTime);
            violetBar.rectTransform.DOSizeDelta(new Vector2(Mathf.Max(minWidth, (float)manaLevels[3] / sum * manaPanelWidth), violetBar.rectTransform.sizeDelta.y), tweenTime);

            redText.SetText(manaLevels[0]);
            greenText.SetText(manaLevels[1]);
            blueText.SetText(manaLevels[2]);
            violetText.SetText(manaLevels[3]);
        }
    }
}