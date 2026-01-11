    using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
namespace Game.Effects.Augmentation
{
    public class UrchinPlatingPassive : Effect, IApplyEffect
    {
        public UrchinPlatingPassive(PieceLogic piece) : base(-1, 1, piece, "effect_urchin_plating_passive")
        {
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(1, Piece)));
        }
    }
}