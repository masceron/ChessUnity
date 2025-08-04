using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Internal
{
    public class CarapaceKill: Action, IInternal
    {
        public CarapaceKill(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(Target);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Destroy(Target);
        }
    }
}