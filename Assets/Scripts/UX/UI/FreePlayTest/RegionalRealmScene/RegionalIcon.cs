using Game.Effects.FieldEffect;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace UX.UI.FreePlayTest.RegionalRealmScene
{
    public class RegionalIcon : MonoBehaviour, IPointerClickHandler
    {
        public TMP_Text tmp;
        public Image image;
        private FieldEffectType _fieldType;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            Choose();
        }

        public void Load(FieldEffectType fieldType)
        {
            tmp.text = AssetManager.Ins.regionalsData.GetRegionalName(fieldType);
            this._fieldType = fieldType;
        }

        public void ResetColor()
        {
            image.color = Color.red;
        }

        public void Choose()
        {
            var previous = RegionalManagerUI.Ins.chosenRegional;
            if (previous == this) return;
            image.color = Color.yellow;
            Config.FieldEffectType = _fieldType;

            if (previous != null) previous.ResetColor();
            RegionalManagerUI.Ins.chosenRegional = this;
        }
    }
}