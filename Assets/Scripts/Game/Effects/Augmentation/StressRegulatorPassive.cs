using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Effects.Augmentation
{
    public class StressRegulatorPassive : Effect, IApplyEffect
    {
        public StressRegulatorPassive(PieceLogic piece) : base(-1, 1, piece, "effect_stress_regulator_passive")
        {
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;
            if (applyEffect.Effect.Piece.SkillCooldown > 0)
            {
                applyEffect.Effect.Piece.SkillCooldown--;
            } 
        }
    }
}