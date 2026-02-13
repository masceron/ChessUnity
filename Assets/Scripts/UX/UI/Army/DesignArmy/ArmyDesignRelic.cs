using System;
using Game.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UX.UI.Army.DesignArmy
{
    public class ArmyDesignRelic : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RawImage image;
        [NonSerialized] public RelicInfo Relic;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            ArmyRelicSearcher.Ins.Select(this);
        }

        public virtual void Load(RelicInfo relicInfo)
        {
            Relic = relicInfo;
            image.texture = relicInfo.icon;
        }
    }
}