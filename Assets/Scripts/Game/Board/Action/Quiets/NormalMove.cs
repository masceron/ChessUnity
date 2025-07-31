using static Game.Board.General.MatchManager;

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
            pieceManager.Move(From, To);
        }

        protected override void ModifyGameState()
        {
            gameState.Move(From, To);
            Caller = To;
        }
    }
}