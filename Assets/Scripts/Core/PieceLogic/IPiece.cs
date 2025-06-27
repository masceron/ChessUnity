using System.Collections.Generic;
using Board.Action;

namespace Core.PieceLogic
{
    public interface IPieceLogic
    { 
        public List<IAction> MoveToMake(int from);
    }
}