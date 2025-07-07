using System.Collections.Generic;
using Board.Action;

namespace Core.PieceLogic
{
    public abstract class PieceLogic
    {
        protected readonly PieceData Piece;

        protected PieceLogic(PieceData p)
        {
            Piece = p;
        }
        public abstract List<Action> MoveToMake(int from);
    }
}