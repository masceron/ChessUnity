using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;

namespace Game.Board.Effects.Others
{
    public class Vengeful: Effect
    {
        public Vengeful(sbyte duration, PieceLogic.PieceLogic piece) : base(duration, 1, piece, EffectType.Vengeful)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.To == Piece.pos && action.Success == ActionResult.Succeed)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(4, MatchManager.GameState.MainBoard[action.To])));
            }
        }
    }
}