using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.Menus
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ButtonEnlarge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            Tween.Scale(transform, 1f, 0.2f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tween.Scale(transform, 0.95f, 0.2f);
        }
    }
}