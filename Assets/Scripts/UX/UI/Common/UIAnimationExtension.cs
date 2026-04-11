using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace UX.UI.Common
{
    public static class UIAnimationExtension
    {
        public static async UniTask AnimateIn(this VisualElement element, string hiddenClass, string showClass, Action onAnimateComplete = null)
        {
            element.AddToClassList(hiddenClass);
            
            element.schedule.Execute(() =>
            {
                element.AddToClassList(showClass);
            });
            await element.WaitForTransitionEnd();

            onAnimateComplete?.Invoke();
        }

        public static async UniTask AnimateIn(this VisualElement element, string toAdd, Action onAnimateComplete = null)
        {
            element.AddToClassList(toAdd);
            await element.WaitForTransitionEnd();
            
            onAnimateComplete?.Invoke();
        }

        public static async UniTask AnimateOut(this VisualElement element, string toRemove, Action onAnimateComplete = null)
        {
            element.RemoveFromClassList(toRemove);
            await element.WaitForTransitionEnd();

            onAnimateComplete?.Invoke();
        }
        
        private static async UniTask WaitForTransitionEnd(this VisualElement element)
        {
            var tcs = new UniTaskCompletionSource();

            EventCallback<TransitionEndEvent> onTransitionEnd = ev =>
            {
                if (ev.target != element) return;
                tcs.TrySetResult();
            };

            element.RegisterCallback(onTransitionEnd);

            try
            {
                await tcs.Task.Timeout(TimeSpan.FromSeconds(0.7));
            }
            catch (TimeoutException)
            {
                UnityEngine.Debug.LogWarning($"[UIAnimation] Transition on '{element.name}' timed out. Check USS setup.");
            }
            finally
            {
                element.UnregisterCallback(onTransitionEnd);
            }
        }
    }
}