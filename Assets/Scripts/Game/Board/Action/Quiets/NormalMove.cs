using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalMove: Action, IQuiets
    {
        public NormalMove(int f, int t) : base(f, true)
        {
            From = (ushort)f;
            To = (ushort)t;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(From, To);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Move(From, To);
            Caller = To;
        }
    }
}