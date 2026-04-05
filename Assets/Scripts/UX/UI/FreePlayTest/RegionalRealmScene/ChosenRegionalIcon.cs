using Game.Common;
using Game.Effects.FieldEffect;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UX.UI.FreePlayTest.RegionalRealmScene
{
    public class ChosenRegionalIcon : Singleton<ChosenRegionalIcon>, IPointerClickHandler
    {
        public TMP_Text tmp;
        public bool isLoaded;
        [FormerlySerializedAs("chosenRegional")] public FieldEffectType chosenField;

        protected override void Awake()
        {
            base.Awake();
            Load(Config.FieldEffectType);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            if (chosenField != FieldEffectType.None)
            {
                Debug.Log("UnLoad regional effect");
                Load(FieldEffectType.None);
            }
        }

        public void Load(FieldEffectType type)
        {
            if (type == FieldEffectType.None)
                tmp.text = "No regional effect";
            else
                tmp.text = AssetManager.Ins.regionalsData.GetRegionalName(type);
            chosenField = type;
            Config.FieldEffectType = type;
        }
    }
}