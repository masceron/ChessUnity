using Board.Piece;
using Core;
using Unity.IL2CPP.CompilerServices;

namespace Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalMove: IAction
    {
        private readonly int _from;
        private readonly int _to;
        private readonly int rankTo;
        private readonly int fileTo;
        
        private readonly PieceManager _pieceManager;

        public NormalMove(int f, int t, PieceManager p, int maxFile)
        {
            _from = f;
            _to = t;
            rankTo = _to / maxFile;
            fileTo = _to % maxFile;
            
            _pieceManager = p;
        }

        public void ApplyAction()
        {
            _pieceManager.GetPiece(_from).Move(rankTo, fileTo);
            _pieceManager.Move(_from, _to);
        }

        public Move MakeEncodedMove()
        {
            return new Move
            {
                from = (byte)_from,
                to = (byte)_to,
                flag = MoveFlag.NormalMove
            };
        }
    }
}