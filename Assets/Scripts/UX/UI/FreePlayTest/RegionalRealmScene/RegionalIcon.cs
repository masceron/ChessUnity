using Game.Effects.RegionalEffect;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UX.UI.FreePlayTest.RegionalRealmScene
{
    public class RegionalIcon : MonoBehaviour, IPointerClickHandler
    {
        public TMP_Text tmp;
        public Image image;
        private RegionalEffectType regionalType;
        public void Load(RegionalEffectType regionalType)
        {
            tmp.text = AssetManager.Ins.regionalsData.GetRegionalName(regionalType);
            this.regionalType = regionalType;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            Choose();
        }
        public void ResetColor()
        {
            image.color = UnityEngine.Color.red;
        }
        public void Choose()
        {
            RegionalIcon previous = RegionalManagerUI.Ins.chosenRegional;
            if (previous == this)
            {
                return;
            }
            image.color = UnityEngine.Color.yellow;
            Config.regionalEffectType = regionalType;
            
            if (previous != null)
            {
                previous.ResetColor();
            }
            RegionalManagerUI.Ins.chosenRegional = this;
        }
    }
}