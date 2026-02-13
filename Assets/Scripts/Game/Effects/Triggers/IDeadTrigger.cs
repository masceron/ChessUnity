using System;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Triggers
{
    public interface IDeadTrigger : IComparable<IDeadTrigger>
    {
        int IComparable<IDeadTrigger>.CompareTo(IDeadTrigger other)
        {
            return 0;
        }

        public void OnCallDead(PieceLogic pieceToDie);
    }
}