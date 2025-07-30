using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Internal
{
    public class DestroyPiece: Action, IInternal
    {
        public DestroyPiece(int caller) : base(caller, false)
        {}

        protected override void Animate()
        {
            pieceManager.Destroy(Caller);
        }

        protected override void ModifyGameState()
        {
            gameState.Destroy(Caller);
        }
    }
}