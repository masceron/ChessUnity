using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.Tooltip
{
    //Gắn vào các ArmyTroop để hiển thị Tooltip khi hover vào quân cờ
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string content;
        private Tween delay;
        private string headerLeft;
        private string headerRight;
        private bool showing;

        private void OnDisable()
        {
            delay.Stop();
            if (showing) TooltipManager.Ins.Hide();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            delay = Tween.Delay(1f, () =>
            {
                TooltipManager.Ins.Show(headerLeft, headerRight, content);
                showing = true;
            });
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