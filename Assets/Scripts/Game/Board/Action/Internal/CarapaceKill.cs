using Game.Board.General;
using Game.Board.Piece;

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
            PieceManager.Ins.Destroy(To);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Destroy(To);
        }
    }
}