using Game.Common;
using Game.Effects;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UX.UI.Ingame.Tooltip;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EffectUI: MonoBehaviour
    {
        [SerializeField] private RawImage icon;
        [SerializeField] private TMP_Text duration;
        [SerializeField] private TooltipTrigger trigger;
        
        public void Set(Effect effect)
        {
            var effectInfo = AssetManager.Ins.EffectData[effect.EffectName];
            duration.text =  effect.Duration != -1 ? effect.Duration.ToString() : "";
            trigger.SetText(effectInfo.effectName, Numerals.ToRomanNumeral(effect.Strength), effect.Description());
            
            icon.texture = effectInfo.icon;
        }
    }
}