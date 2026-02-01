using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class CursedModulePassive : Effect, IOnApply
    {
        private const int cooldownDecrease = 4;
        public CursedModulePassive(PieceLogic piece) : base(-1, 1, piece, "effect_cursed_module_passive")
        {
        }

        public void OnApply()
        {
            if (Piece.SkillCooldown >= cooldownDecrease)
            {
                Piece.SkillCooldown -= cooldownDecrease;
            }
            else Piece.SkillCooldown = 0;
            
            ActionManager.EnqueueAction(new ApplyEffect(new Demolisher(Piece)));
        }
    }
}