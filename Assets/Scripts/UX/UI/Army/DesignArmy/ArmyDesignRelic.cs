using System;
using Game.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UX.UI.Army.DesignArmy
{
    public class ArmyDesignRelic: MonoBehaviour, IPointerClickHandler
    {
        [NonSerialized] public RelicInfo Relic;
        [SerializeField] private RawImage image;
        
        public void Load(RelicInfo relicInfo)
        {
            Relic = relicInfo;
            image.texture = relicInfo.icon;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            ArmyRelicSearcher.Ins.Select(this);
        }
    }
}