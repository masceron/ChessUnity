using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;

namespace Game.Board.Effects
{
    public class Carapace: Effect
    {
        public Carapace(sbyte duration, sbyte strength, PieceLogic.PieceLogic piece) : base(duration, strength, piece, ObserverType.Captures, ObserverPriority.Buff)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.To != Piece.pos || action.Success != ActionResult.UNKNOWN) return;

            action.Success = MatchManager.Roll(Strength) ? ActionResult.SUCCEED : ActionResult.FAILED;
            ActionManager.TakeAction(new CarapaceKill(Piece.pos, action.To));
        }
    }
}