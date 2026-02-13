using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class FocusModulePassive : Effect, IOnApplyTrigger
    {
        public FocusModulePassive(PieceLogic piece) : base(-1, 1, piece, "effect_focus_module_passive")
        {
        }
        
        public void OnApply()
        {
            switch (Piece.SkillCooldown)
            {
                case > 1:
                    Piece.SkillCooldown -= 2;
                    break;
                case 1:
                    Piece.SkillCooldown -= 1;
                    break;
            }
        }
    }
}