using Game.Action;
using Game.Action.Internal;
using Game.Effects.RegionalEffect;
using Game.Managers;
using System.Linq;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.Effects.Condition
{
    public class NativeGround : Effect
    {
        private readonly RegionalEffectType regionalEffect;
        public NativeGround(PieceLogic piece, RegionalEffectType fitRegionalEffect) : base(-1, 1, piece, "effect_native_ground")
        {
            regionalEffect = fitRegionalEffect;

            ApplyEffectIfFitRegion();
        }

        private void ApplyEffectIfFitRegion() 
        {
            if (regionalEffect != MatchManager.Ins.GameState.RegionalEffect.Type) return;

            var e = GetRandomBuffEffect();

            ActionManager.ExecuteImmediately(new ApplyEffect(e));
            Debug.Log($"Receive effect {e.EffectName} duration = {e.Duration}");
        }

        private Effect GetRandomBuffEffect()
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();

            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];

            return CreateEffectFromName(selectedEffectName, Piece);
        }

        private Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var duration = (sbyte)Random.Range(1, 10);
            sbyte strength = 1;

            return effectName switch
            {
                "effect_shield" => new Buffs.Shield(piece),
                "effect_carapace" => new Buffs.Carapace(duration, piece),
                "effect_haste" => new Buffs.Haste(duration, strength, piece),
                "effect_piercing" => new Buffs.Piercing(duration, piece),
                "effect_hardened_shield" => new Buffs.HardenedShield(piece),
                "effect_true_bite" => new Buffs.TrueBite(piece),
                "effect_camouflage" => new Buffs.Camouflage(piece),
                _ => new Buffs.Shield(piece)
            };
        }
    }
}

