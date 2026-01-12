using Game.Action.Internal;
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
            if (applyEffect.Effect.EffectName == "effect_shortreach")
            {
                applyEffect.Result = Action.ResultFlag.EffectResistance;
            }
        }
        
    }
}