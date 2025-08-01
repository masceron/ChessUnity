using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Internal
{
    public class CarapaceKill: Action, IInternal
    {
        public CarapaceKill(int caller, int to) : base(caller, false)
        {
            From = (ushort)caller;
            To = (ushort)to;
        }

        protected override void Animate()
        {
            pieceManager.Destroy(To);
        }

        protected override void ModifyGameState()
        {
            gameState.Destroy(To);
        }
    }
}