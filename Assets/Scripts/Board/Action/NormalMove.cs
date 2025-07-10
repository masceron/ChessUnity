using Board.Piece;
using Core;
using Core.General;
using Unity.IL2CPP.CompilerServices;

namespace Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalMove: Action
    {
        private readonly PieceManager pieceManager;

        public NormalMove(ushort caller, int f, int t, PieceManager p) : base(caller, true, false)
        {
            From = (ushort)f;
            To = (ushort)t;
            
            pieceManager = p;
        }

        public override void ApplyAction(GameState state)
        {
            pieceManager.Move(From, To);
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.Move(From, To);
            state.LastMove = this;
            Caller = To;
        }
    }
}