using System;
using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;

namespace Game.Triggers
{
    public interface IOnMoveGenTrigger : IComparable<IOnMoveGenTrigger>
    {
        int IComparable<IOnMoveGenTrigger>.CompareTo(IOnMoveGenTrigger other)
        {
            return 0;
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions);
    }
}