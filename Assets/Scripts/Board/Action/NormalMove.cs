using Board.Piece;
using Core;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalMove: IAction
    {
        private readonly int from;
        private readonly int to;
        private readonly int rankTo;
        private readonly int fileTo;
        
        private readonly PieceManager _pieceManager;

        public NormalMove(int f, int t, PieceManager p, int maxFile)
        {
            from = f;
            to = t;
            rankTo = to / maxFile;
            fileTo = to % maxFile;
            
            _pieceManager = p;
        }

        public bool IsLegal()
        {
            return true;
        }

        public void ApplyAction()
        {
            _pieceManager.GetPiece(from).Move(rankTo, fileTo);
            _pieceManager.Move(from, to);
        }

        public Move MakeEncodedMove()
        {
            Move move;
            move.from_to = (ushort)(from + (to << 8));
            move.flag = 0;

            return move;
        }
    }
}