
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class SpectreAugmentation : Effect, IApplyEffect
    {
        public SpectreAugmentation(PieceLogic piece) : base(-1, 1, piece, "effect_spectre_augmentation")
        {
        }
        
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Category == EffectCategory.Debuff && applyEffect.SourcePiece == Piece)
            {
                // TODO: Get random buff effect when this piece owns spectre augmentation.
                // Just one buff effect for each turn. 
            }
        }
    }
}