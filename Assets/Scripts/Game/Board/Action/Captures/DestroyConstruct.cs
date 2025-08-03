using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Captures
{
    public class DestroyConstruct: Action, ICaptures
    {
        public DestroyConstruct(int from, int to) : base(from, false)
        {
            To = (ushort)to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(From);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Destroy(From);
            MatchManager.Ins.GameState.Destroy(To);
        }
    }
}