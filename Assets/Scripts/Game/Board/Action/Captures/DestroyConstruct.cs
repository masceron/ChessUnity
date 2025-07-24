using static Game.Board.General.MatchManager;

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
            pieceManager.Destroy(Caller);
        }

        protected override void ModifyGameState()
        {
            gameState.Destroy(Caller);
            gameState.Destroy(To);
            gameState.ActiveBoard[To] = true;
        }
    }
}