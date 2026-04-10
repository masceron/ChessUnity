using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class StressRegulatorPassive : Effect, IOnApplyTrigger
    {
        public StressRegulatorPassive(PieceLogic piece) : base(-1, 1, piece, "effect_stress_regulator_passive")
        {
        }

        public void OnApply()
        {
            //Làm lại
            //if (Piece.SkillCooldown > 0) Piece.SkillCooldown--;
        }
    }
}