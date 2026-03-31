using Game.Action;
using Game.Action.Captures;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SlayersCoin : Effect, IAfterPieceActionTrigger
    {
        public SlayersCoin(PieceLogic piece) : base(-1, 1, piece, "effect_slayers_coin")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Other;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Result != ResultFlag.Success) return;

            var caller = action.GetMakerAsPiece();
            var captured = action.GetTargetAsPiece();

            if (caller.Color == Piece.Color && caller.PieceRank < captured.PieceRank) ((Chrysos)Piece).Coin += 1;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 100;
        }
    }
}