using UnityEngine;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using CharTween;
using Doragon.Logging;

namespace Doragon.UI
{
    public class AnimateLoading : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private CanvasGroup canvasGroup;
        private const int fadeTime = 1;
        public async UniTask Animate()
        {
            await canvasGroup.DOFade(1, fadeTime);

            var sequence = DOTween.Sequence();
            var tweener = loadingText.GetCharTweener();
            // TODO: make this a global tween method => bouncing text
            for (int i = 0; i <= loadingText.text.Length; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 1, (i) / (float)(loadingText.text.Length + 1));
                var charSequence = DOTween.Sequence();
#pragma warning disable 4014
                charSequence.Append(tweener.DOLocalMoveY(i, 0.5f, 0.5f).SetEase(Ease.InOutCubic))
                    .Join(tweener.DOScale(i, 0, 0.4f).From().SetEase(Ease.OutBack, 4))
                    .Append(tweener.DOLocalMoveY(i, 0, 0.4f).SetEase(Ease.OutBounce));
                sequence.Insert(timeOffset, charSequence);
            }
            sequence.SetLoops(-1, LoopType.Restart);
#pragma warning restore 4014
            await UniTask.Delay(1000);
            //
        }

        public async UniTask StopAnimating()
        {
            await canvasGroup.DOFade(0, fadeTime);
        }
    }
}