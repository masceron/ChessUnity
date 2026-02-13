using System;
using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Triggers
{
    public interface IOnMoveGenTrigger: IComparable<IOnMoveGenTrigger>
    {
        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions);

        int IComparable<IOnMoveGenTrigger>.CompareTo(IOnMoveGenTrigger other)
        {
            return 0;
        }
    }
}