using System.Linq;
using Game.Action.Internal;
using Game.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class PressureMembranePassive : Effect, IMoveRangeModifier, IApplyEffect
    {
        private const int moveRangeModifier = 2;
        
        public PressureMembranePassive(PieceLogic piece) : base(-1, 1, piece, "effect_pressure_membrane_passive")
        {
        }

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + moveRangeModifier;
        }
        
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            PieceLogic pieceApplied = applyEffect.Effect.Piece;
            if (pieceApplied.Augmentations.Any(aug => aug.Name == AugmentationName.PressureMembrane) 
                && applyEffect.Effect.EffectName == "effect_shortreach")
            {
                applyEffect.Result = Action.ResultFlag.Blocked;
            }
        }
        
    }
}