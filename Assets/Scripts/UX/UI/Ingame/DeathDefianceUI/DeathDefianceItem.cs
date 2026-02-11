using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.Managers;

namespace UX.UI.Ingame.DeathDefianceUI  
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DeathDefianceItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text effectNameText;
        // [SerializeField] private TMP_Text effectDescriptionText;
        // [SerializeField] private Button selectButton;
        [SerializeField] private RawImage effectIcon;
        
        private string effectName;
        // private DeathDefianceUI parentUI;
        
        public void Load(string effect)
        {
            effectName = effect;
            var info = AssetManager.Ins.EffectData[effect];
            effectNameText.text = Localizer.GetText("effect_name", info.key, null);
            effectIcon.texture = info.icon;
        }

        public void Choose()
        {
            transform.parent.parent.GetComponent<DeathDefiancePendingMenu>().ChooseEffect(effectName);
        }
        

    }
}
