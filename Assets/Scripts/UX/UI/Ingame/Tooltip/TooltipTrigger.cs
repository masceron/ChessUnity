using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.Ingame.Tooltip
{
    public class TooltipTrigger: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string headerLeft;
        private string headerRight;
        private string content;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Ins.Show(headerLeft, headerRight, content);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Ins.Hide();
        }

        public void SetText(string left, string right, string cnt)
        {
            this.headerLeft = left;
            this.headerRight = right;
            content = cnt;
        }
    }
}