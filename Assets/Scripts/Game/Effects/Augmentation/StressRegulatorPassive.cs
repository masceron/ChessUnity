using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class StressRegulatorPassive : Effect, IOnApplyTrigger
    {
        public StressRegulatorPassive(PieceLogic piece) : base(-1, 1, piece, "effect_stress_regulator_passive")
        {
        }

        public void OnApply()
        {
            if (Piece.SkillCooldown > 0) Piece.SkillCooldown--;
        }
    }
}