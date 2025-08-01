using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalCapture: Action, ICaptures
    {
        public NormalCapture(int f, int t) : base(f, true)
        {
            From = (ushort)f;
            To = (ushort)t;
        }
        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(To);
            PieceManager.Ins.Move(From, To);
            MatchManager.Ins.GameState.Destroy(To);
            MatchManager.Ins.GameState.Move(From, To);
            Caller = To;
        }
    }
}