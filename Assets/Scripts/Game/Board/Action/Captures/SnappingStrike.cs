using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Captures
{
    public class SnappingStrike: Action, ICaptures
    {
        public SnappingStrike(int caller, int to) : base(caller, false)
        {
            From = (ushort)caller;
            To = (ushort)to;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            pieceManager.Destroy(To);
            gameState.Destroy(To);
        }
    }
}