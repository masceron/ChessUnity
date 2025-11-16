using UnityEngine;

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