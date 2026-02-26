using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class ReefPlatingPassive : Effect, IOnApplyTrigger
    {
        public ReefPlatingPassive(PieceLogic piece) : base(-1, 1, piece, "effect_reef_plating_passive")
        {
        }


        public void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(Piece)));
        }
    }
}