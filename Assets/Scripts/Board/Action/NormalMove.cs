using Board.Piece;
using Core;
using Unity.IL2CPP.CompilerServices;

namespace Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalMove: Action
    {
        private readonly PieceManager _pieceManager;

        public NormalMove(int f, int t, PieceManager p, int maxFile)
        {
            From = f;
            To = t;
            
            _pieceManager = p;

            Move = new Move
            {
                from = (byte)From,
                to = (byte)To,
                flag = MoveFlag.NormalMove
            };
        }

        public override void ApplyAction()
        {
            _pieceManager.Move(From, To);
        }
    }
}