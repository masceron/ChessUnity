using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.Menus
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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