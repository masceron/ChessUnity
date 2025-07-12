using Game.Board.General;
using Game.Board.Interaction;
using Game.Board.Piece;

namespace Game.Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalMove: Action
    {
        public NormalMove(ushort caller, int f, int t) : base(caller, true, false, false)
        {
            From = (ushort)f;
            To = (ushort)t;
        }

        public override void ApplyAction(GameState state)
        {
            InteractionManager.PieceManager.Move(From, To);
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.Move(From, To);
            Caller = To;
        }
    }
}