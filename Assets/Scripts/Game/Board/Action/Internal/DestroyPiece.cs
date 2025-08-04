using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Internal
{
    public class DestroyPiece: Action, IInternal
    {
        public DestroyPiece(int maker) : base(maker)
        {}

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(Maker);
            MatchManager.Ins.GameState.Destroy(Maker);
        }
    }
}