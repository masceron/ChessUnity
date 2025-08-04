using Game.Board.General;
using Game.Board.Piece;

namespace Game.Board.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalCapture: Action, ICaptures
    {
        public NormalCapture(int f, int t) : base(f, true)
        {
            Maker = (ushort)f;
            Target = (ushort)t;
        }
        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(Target);
            PieceManager.Ins.Move(Maker, Target);
            MatchManager.Ins.GameState.Destroy(Target);
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }
    }
}