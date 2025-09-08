using System;
using Game.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UX.UI.Followers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RelicLogo: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private RawImage image;
        [NonSerialized] public RelicInfo Relic;
        private bool selecting;

        public void Load(RelicInfo info)
        {
            Relic = info;
            image.texture = info.icon;
        }
        
        public void Undisplay()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            RelicList.Ins.DisplayInfo(Relic);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            RelicList.Ins.Undisplay();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                RelicList.Ins.Select(Relic);
            }
        }
    }
}