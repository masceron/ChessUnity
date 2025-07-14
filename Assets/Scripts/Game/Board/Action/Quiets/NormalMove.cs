using Game.Board.General;
using Game.Board.Interaction;

namespace Game.Board.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalMove: Action, IQuiets
    {
        public NormalMove(ushort caller, int f, int t) : base(caller, true)
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