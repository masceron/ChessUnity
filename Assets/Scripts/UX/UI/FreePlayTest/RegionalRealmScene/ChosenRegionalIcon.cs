using Game.Common;
using Game.Effects.RegionalEffect;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UX.UI.FreePlayTest.RegionalRealmScene
{
    public class ChosenRegionalIcon : Singleton<ChosenRegionalIcon>, IPointerClickHandler
    {
        public TMP_Text tmp;
        public bool isLoaded;
        public RegionalEffectType chosenRegional;

        protected override void Awake()
        {
            base.Awake();
            Load(Config.regionalEffectType);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            if (chosenRegional != RegionalEffectType.None)
            {
                Debug.Log("UnLoad regional effect");
                Load(RegionalEffectType.None);
            }
        }

        public void Load(RegionalEffectType type)
        {
            if (type == RegionalEffectType.None)
                tmp.text = "No regional effect";
            else
                tmp.text = AssetManager.Ins.regionalsData.GetRegionalName(type);
            chosenRegional = type;
            Config.regionalEffectType = type;
        }
    }
}