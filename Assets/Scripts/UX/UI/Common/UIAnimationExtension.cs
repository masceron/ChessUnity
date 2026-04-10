using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace UX.UI.Common
{
    public static class UIAnimationExtension
    {
        public static async UniTask AnimateIn(this VisualElement element, string hiddenClass, string showClass,
            int duration, Action onAnimateComplete = null)
        {
            element.AddToClassList(hiddenClass);

            await UniTask.DelayFrame(2);
            element.AddToClassList(showClass);
            await UniTask.Delay(duration, ignoreTimeScale: true);

            onAnimateComplete?.Invoke();
        }

        public static async UniTask AnimateIn(this VisualElement element, string toAdd, int duration,
            Action onAnimateComplete = null)
        {
            element.AddToClassList(toAdd);
            await UniTask.Delay(duration, ignoreTimeScale: true);
            
            onAnimateComplete?.Invoke();
        }

        public static async UniTask AnimateOut(this VisualElement element, string toRemove, int duration,
            Action onAnimateComplete = null)
        {
            element.RemoveFromClassList(toRemove);
            await UniTask.Delay(duration, ignoreTimeScale: true);

            onAnimateComplete?.Invoke();
        }
    }
}