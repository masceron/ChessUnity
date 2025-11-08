using System;
using Game.ScriptableObjects;
using UI.UIObject3D.Scripts;
using Game.Augmentation;
using Game.Save.Army;
using UnityEngine;
using UnityEngine.UI;

using Game.Effects.RegionalEffect;
using TMPro;
using Game.Managers;
using UnityEngine.EventSystems;
namespace UX.UI.FreePlayTest
{
    public class RegionalIcon : MonoBehaviour, IPointerClickHandler
    {
        public TMP_Text tmp;
        private RegionalEffectType regionalType;
        public void Load(RegionalEffectType regionalType)
        {
            tmp.text = AssetManager.Ins.RegionalsData.GetRegionalName(regionalType);
            this.regionalType = regionalType;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            RegionalManagerUI.Ins.chosenRegional.Load(regionalType);

    
        }
    }
}