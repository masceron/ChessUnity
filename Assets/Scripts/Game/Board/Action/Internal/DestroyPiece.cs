using static Game.Board.General.MatchManager;

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
            pieceManager.Destroy(Caller);
            gameState.Destroy(Caller);
        }
    }
}