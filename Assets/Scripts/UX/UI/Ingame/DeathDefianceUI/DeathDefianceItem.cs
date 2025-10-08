using Game.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UX.UI.Ingame.DeathDefianceUI;
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
        
        private EffectName effectName;
        // private DeathDefianceUI parentUI;
        
        public void Load(EffectName effect)
        {
            effectName = effect;
            // parentUI = parent;
            var info = AssetManager.Ins.EffectData[effect];
            effectNameText.text = Localizer.GetText("effect_name", info.key, null);
            effectIcon.texture = info.icon;
            // effectNameText.text = effect.ToString();
            // effectDescriptionText.text = GetEffectDescription(effect);
            
            // selectButton.onClick.RemoveAllListeners();
            // selectButton.onClick.AddListener(() => parentUI.ChooseEffect(effectName));
        }

        public void Choose()
        {
            transform.parent.parent.GetComponent<DeathDefianceUI>().ChooseEffect(effectName);
        }
        

    }
}
