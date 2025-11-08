using Game.Common;
using Game.Effects.RegionalEffect;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Game.Managers;
using Game.Effects.RegionalEffect;

namespace UX.UI.FreePlayTest
{
    public class ChosenRegionalIcon : Singleton<ChosenRegionalIcon>, IPointerClickHandler
    {
        public TMP_Text tmp;
        public bool isLoaded = false;
        public RegionalEffectType chosenRegional;
        protected override void Awake()
        {
            base.Awake();
            Load(Config.regionalEffectType);
        }
        public void Load(RegionalEffectType type)
        {
            if (type == RegionalEffectType.None)
            {
                tmp.text = "No regional effect";
            }
            else
            {
                tmp.text = AssetManager.Ins.RegionalsData.GetRegionalName(type);
                
            }
            this.chosenRegional = type;
            Config.regionalEffectType = type;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            if (chosenRegional != RegionalEffectType.None)
            {
                Debug.Log("UnLoad regional effect");
                Load(RegionalEffectType.None);
            }
            else
            {
                
            }
        }
    }
}