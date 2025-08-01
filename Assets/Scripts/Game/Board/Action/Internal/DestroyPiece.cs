using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Internal
{
    public class DestroyPiece: Action, IInternal
    {
        public DestroyPiece(int caller) : base(caller, false)
        {}

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(Caller);
            MatchManager.Ins.GameState.Destroy(Caller);
        }
    }
}