using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
namespace Game.Effects.Augmentation
{
    public class AdaptiveGlandPassive : Effect, IApplyEffect
    {
        public AdaptiveGlandPassive(PieceLogic piece) : base(-1, 1, piece, "effect_adaptive_gland_passive")
        {
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Adaptation(Piece)));
        }
    }
}