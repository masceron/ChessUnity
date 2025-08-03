using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Captures
{
    public class SnappingStrike: Action, ICaptures
    {
        public SnappingStrike(int from, int to) : base(from, false)
        {
            From = (ushort)from;
            To = (ushort)to;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(To);
            MatchManager.Ins.GameState.Destroy(To);
        }
    }
}