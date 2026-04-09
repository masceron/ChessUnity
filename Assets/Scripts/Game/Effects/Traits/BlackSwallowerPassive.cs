using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Others;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlackSwallowerPassive : Effect, IAfterPieceActionTrigger
    {
        public BlackSwallowerPassive(PieceLogic piece) : base(-1, 1, piece, "effect_black_swallower_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Kill;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;

            if (action.Result != ResultFlag.Success) return;

            if (action.GetMakerAsPiece() != Piece) return;

            var targetPiece = action.GetTargetAsPiece();
            if (targetPiece is { PieceRank: PieceRank.Elite or PieceRank.Champion or PieceRank.Commander })
                ActionManager.EnqueueAction(new ApplyEffect(new KillPieceAfterSwitchTurn(Piece), Piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40;
        }
    }
}