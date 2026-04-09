using System;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;

namespace Game.Effects.Augmentation
{
    public class SpectreAugmentation : Effect, IBeforeApplyEffectTrigger
    {
        private int _buffForEachTurn = 1;
        private int _lastProcessedTurn = -1;

        public SpectreAugmentation(PieceLogic piece) : base(-1, 1, piece, "effect_spectre_augmentation")
        {
        }

        public BeforeApplyEffectTriggerPriority Priority => BeforeApplyEffectTriggerPriority.Reaction;

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            var currentTurn = BoardUtils.GetCurrentTurn();

            if (currentTurn != _lastProcessedTurn)
            {
                _lastProcessedTurn = currentTurn;
                _buffForEachTurn = 1;
            }

            if (applyEffect.Effect.Category != EffectCategory.Debuff || applyEffect.GetMakerAsPiece() != Piece ||
                _buffForEachTurn <= 0) return;
            
            ActionManager.EnqueueAction(new ApplyEffect(GetRandomBuffEffect(Piece), Piece));
            _buffForEachTurn--;
        }

        private Effect GetRandomBuffEffect(PieceLogic piece)
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();

            var random = new Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];

            return CreateEffectFromName(selectedEffectName, piece);
        }

        private Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var randomDuration = new Random().Next(2, 6);

            return effectName switch
            {
                "effect_shield" => new Shield(piece),
                "effect_carapace" => new Carapace(randomDuration, piece),
                "effect_haste" => new Haste(randomDuration, 1, piece),
                "effect_piercing" => new Piercing(randomDuration, piece),
                "effect_hardened_shield" => new HardenedShield(piece),
                "effect_true_bite" => new TrueBite(-1 ,piece),
                "effect_camouflage" => new Camouflage(piece),
                _ => null
            };
        }
    }
}