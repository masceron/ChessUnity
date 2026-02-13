    using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects.Triggers;

namespace Game.Effects.Augmentation
{
    public class UrchinPlatingPassive : Effect, IOnApplyTrigger
    {
        public UrchinPlatingPassive(PieceLogic piece) : base(-1, 1, piece, "effect_urchin_plating_passive")
        {
        }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(-1, Piece)));
        }
    }
}