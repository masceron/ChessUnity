using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Buffs;
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
            this.regionalEffect = fitRegionalEffect;

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
            sbyte duration = (sbyte)UnityEngine.Random.Range(1, 10);
            sbyte strength = 1;

            return effectName switch
            {
                EffectName.Shield => new Game.Effects.Buffs.Shield(piece),
                EffectName.Carapace => new Game.Effects.Buffs.Carapace(duration, piece),
                EffectName.Haste => new Game.Effects.Buffs.Haste(duration, strength, piece),
                EffectName.Piercing => new Game.Effects.Buffs.Piercing(duration, piece),
                EffectName.HardenedShield => new Game.Effects.Buffs.HardenedShield(piece),
                EffectName.TrueBite => new Game.Effects.Buffs.TrueBite(piece),
                EffectName.Camouflage => new Game.Effects.Buffs.Camouflage(piece),
                _ => new Game.Effects.Buffs.Shield(piece)
            };
        }
    }
}

