using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.Menus
{
    public class ButtonEnlarge: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1f, 0.2f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(0.95f, 0.2f);
        }
    }
}