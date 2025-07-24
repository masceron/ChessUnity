using static Game.Board.General.MatchManager;

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
            pieceManager.Destroy(To);
            pieceManager.Move(From, To);
            gameState.Destroy(To);
            gameState.Move(From, To);
            Caller = To;
        }
    }
}