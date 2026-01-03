using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects
{
    public interface IOnMoveGenEffect
    {
        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions);
    }
}