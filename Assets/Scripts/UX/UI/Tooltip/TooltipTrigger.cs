using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.Tooltip
{
    public class TooltipTrigger: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string headerLeft;
        private string headerRight;
        private string content;
        private Tween delay;
        private bool showing;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            delay = Tween.Delay(1f, () =>
            {
                TooltipManager.Ins.Show(headerLeft, headerRight, content);
                showing = true;
            });
        }

        private void OnDisable()
        {
            delay.Stop();
            if (showing)
            {
                TooltipManager.Ins.Hide();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            delay.Stop();
            TooltipManager.Ins.Hide();
            showing = false;
        }

        public void SetText(string left, string right, string cnt)
        {
            headerLeft = left;
            headerRight = right;
            content = cnt;
        }
    }
}