using Board.Piece;
using Core;
using Unity.IL2CPP.CompilerServices;

namespace Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalMove: Action
    {
        private readonly PieceManager pieceManager;

        public NormalMove(int caller, int f, int t, PieceManager p) : base(caller)
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
            state.LastMovedPiece = state.MainBoard[From];
            state.MainBoard[To] = state.MainBoard[From];
            state.MainBoard[To].Pos = To;
            state.MainBoard[From] = null;
            state.LastMove = this;
        }

        public override bool DoesMoveChangePos()
        {
            return true;
        }
    }
}