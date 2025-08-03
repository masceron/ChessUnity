using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Internal
{
    public class DestroyPiece: Action, IInternal
    {
        public DestroyPiece(int from) : base(from, false)
        {}

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(From);
            MatchManager.Ins.GameState.Destroy(From);
        }
    }
}