using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Captures
{
    public class DestroyConstruct: Action, ICaptures
    {
        public DestroyConstruct(int caller, int to) : base(caller, false)
        {
            To = (ushort)to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(Caller);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Destroy(Caller);
            MatchManager.Ins.GameState.Destroy(To);
            MatchManager.Ins.GameState.ActiveBoard[To] = true;
        }
    }
}