
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Effects.Others
{
    public class SpectreAugmentation : Effect, IApplyEffect
    {
        private int buffForEachTurn = 1;
        private int lastProcessedTurn = -1;
        
        public SpectreAugmentation(PieceLogic piece) : base(-1, 1, piece, "effect_spectre_augmentation")
        {
        }
        
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            int currentTurn = MatchManager.Ins.GameState.CurrentTurn;

            if (currentTurn != lastProcessedTurn)
            {
                lastProcessedTurn = currentTurn;
                buffForEachTurn = 1;
            }
            
            if (applyEffect.Effect.Category == EffectCategory.Debuff && applyEffect.SourcePiece == Piece && buffForEachTurn > 0)
            {
                ActionManager.EnqueueAction(new ApplyEffect(GetRandomBuffEffect(Piece), Piece));
                buffForEachTurn--;
            }
        }
        
        private Effect GetRandomBuffEffect(PieceLogic piece)
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            
            return CreateEffectFromName(selectedEffectName, piece);
        }

        private Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var randomDuration = (sbyte)new System.Random().Next(2, 6);

            return effectName switch
            {
                "effect_shield" => new Effects.Buffs.Shield(piece),
                "effect_carapace" => new Effects.Buffs.Carapace(randomDuration, piece),
                "effect_haste" => new Effects.Buffs.Haste(randomDuration, 1, piece),
                "effect_piercing" => new Effects.Buffs.Piercing(randomDuration, piece),
                "effect_hardened_shield" => new Effects.Buffs.HardenedShield(piece),
                "effect_true_bite" => new Effects.Buffs.TrueBite(piece),
                "effect_camouflage" => new Effects.Buffs.Camouflage(piece),
                _ => null
            };
        }
    }
}