using Game.Board.Effects;
using Game.Board.General;
using Game.Common;
using Simple_Tooltip.Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EffectUI: MonoBehaviour
    {
        [SerializeField] private RawImage icon;
        [SerializeField] private TMP_Text strength;
        [SerializeField] private TMP_Text duration;
        [SerializeField] private SimpleTooltip tooltip;
        
        public void Set(Effect effect)
        {
            var effectInfo = MatchManager.assetManager.EffectData[effect.EffectName];
            strength.text = effect.Strength > 1 ? effect.Strength.ToString() : "";
            duration.text =  effect.Duration != -1 ? effect.Duration.ToString() : "";
            
            if (effectInfo.icon != null) icon.texture = effectInfo.icon;
            
            tooltip.infoLeft = "~" + effectInfo.effectName
                               + (effect.Strength > 1 ? " " + Numerals.ToRomanNumeral(effect.Strength) : "")
                               + "\n`"
                               + effect.Description();

            if (effectInfo.category == EffectCategory.Trait) tooltip.infoRight = "Trait";
            else tooltip.infoRight = "`" + (effect.Duration != -1 ? effect.Duration.ToString() : "∞");
        }
    }
}