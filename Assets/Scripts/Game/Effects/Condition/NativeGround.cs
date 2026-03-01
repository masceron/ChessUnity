using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.RegionalEffect;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using ZLinq;
using Random = System.Random;

namespace Game.Effects.Condition
{
    public class NativeGround : Effect
    {
        private readonly RegionalEffectType regionalEffect;

        public NativeGround(PieceLogic piece, RegionalEffectType fitRegionalEffect) : base(-1, 1, piece,
            "effect_native_ground")
        {
            regionalEffect = fitRegionalEffect;

            ApplyEffectIfFitRegion();
        }

        private void ApplyEffectIfFitRegion()
        {
            if (regionalEffect != MatchManager.Ins.GameState.RegionalEffect.Type) return;

            var e = GetRandomBuffEffect();

            ActionManager.EnqueueAction(new ApplyEffect(e, Piece));
            Debug.Log($"Receive effect {e.EffectName} duration = {e.Duration}");
        }

        private Effect GetRandomBuffEffect()
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();

            var random = new Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];

            return CreateEffectFromName(selectedEffectName, Piece);
        }

        private Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var duration = UnityEngine.Random.Range(1, 11);
            var strength = 1;

            return effectName switch
            {
                "effect_shield" => new Shield(piece),
                "effect_carapace" => new Carapace(duration, piece),
                "effect_haste" => new Haste(duration, strength, piece),
                "effect_piercing" => new Piercing(duration, piece),
                "effect_hardened_shield" => new HardenedShield(piece),
                "effect_true_bite" => new TrueBite(-1 ,piece),
                "effect_camouflage" => new Camouflage(piece),
                _ => new Shield(piece)
            };
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}