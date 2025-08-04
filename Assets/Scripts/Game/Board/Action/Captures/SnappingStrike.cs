using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Captures
{
    public class SnappingStrike: Action, ICaptures
    {
        public SnappingStrike(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(Target);
            MatchManager.Ins.GameState.Destroy(Target);
        }
    }
}