using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PenpointGunnelPassive : Effect, IAfterPieceActionTrigger
    {
        public PenpointGunnelPassive(PieceLogic piece) : base(-1, 1, piece, "effect_penpoint_gunnel_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.Target == Piece.Pos && action.Result == ResultFlag.Success)
                ActionManager.EnqueueAction(
                    new ApplyEffect(new Leashed(BoardUtils.PieceOn(action.Maker), Piece.Pos, -1), Piece));
        }
    }
}