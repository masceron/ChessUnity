using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class CursedModulePassive : Effect, IOnApplyTrigger
    {
        private const int cooldownDecrease = 4;
        public CursedModulePassive(PieceLogic piece) : base(-1, 1, piece, "effect_cursed_module_passive")
        {
        }

        public void OnApply()
        {
            if (Piece is IPieceWithSkill skillPiece)
            {
                if (skillPiece.TimeToCooldown >= cooldownDecrease)
                {
                    skillPiece.TimeToCooldown -= cooldownDecrease;
                }
                else skillPiece.TimeToCooldown = 0;
            }

            ActionManager.EnqueueAction(new ApplyEffect(new Demolisher(Piece)));
        }
    }
}