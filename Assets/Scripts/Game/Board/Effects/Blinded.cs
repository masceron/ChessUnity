using Game.Board.Action;
using Game.Board.General;

namespace Game.Board.Effects
{
    public class Blinded: Effect
    {
        public Blinded(sbyte duration, sbyte strength, PieceLogic.PieceLogic piece) : base(duration, strength, piece, ObserverType.Captures, ObserverPriority.Kill)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.Caller == Piece.pos && action.Success == ActionResult.UNKNOWN)
            {
                action.Success = MatchManager.Roll(Strength) ? ActionResult.SUCCEED : ActionResult.FAILED;
            }
        }
    }
}