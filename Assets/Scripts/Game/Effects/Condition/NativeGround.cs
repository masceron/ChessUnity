using Game.Action;
using Game.Action.Internal;
using Game.Effects.RegionalEffect;
using Game.Managers;
using Game.Piece.PieceLogic;
using System.Linq;
using UnityEngine;

namespace Game.Effects.Condition
{
    public class NativeGround : Effect
    {
        private RegionalEffectType regionalEffect;
        public NativeGround(PieceLogic piece, RegionalEffectType fitRegionalEffect) : base(-1, 1, piece, EffectName.NativeGround)
        {
            regionalEffect = fitRegionalEffect;

            ApplyEffectIfFitRegion();
        }

        private void ApplyEffectIfFitRegion() 
        {
            if (regionalEffect != MatchManager.Ins.GameState.RegionalEffect.Type) return;

            var e = GetRandomBuffEffect();

            ActionManager.ExecuteImmediately(new ApplyEffect(e));
            Debug.Log($"Recive effect {e.EffectName} duration = {e.Duration}");
        }

        public Effect GetRandomBuffEffect()
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();

            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];

            return CreateEffectFromName(selectedEffectName, Piece);
        }

        private Effect CreateEffectFromName(EffectName effectName, PieceLogic piece)
        {
            sbyte duration = (sbyte)Random.Range(1, 10);
            sbyte strength = 1;

            return effectName switch
            {
                EffectName.Shield => new Buffs.Shield(piece),
                EffectName.Carapace => new Buffs.Carapace(duration, piece),
                EffectName.Haste => new Buffs.Haste(duration, strength, piece),
                EffectName.Piercing => new Buffs.Piercing(duration, piece),
                EffectName.HardenedShield => new Buffs.HardenedShield(piece),
                EffectName.TrueBite => new Buffs.TrueBite(piece),
                EffectName.Camouflage => new Buffs.Camouflage(piece),
                _ => new Buffs.Shield(piece)
            };
        }
    }
}

