using System;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Triggers
{
    public interface IDeadTrigger: IComparable<IDeadTrigger>
    {
        public void OnCallDead(PieceLogic pieceToDie);

        int IComparable<IDeadTrigger>.CompareTo(IDeadTrigger other)
        {
            return 0;
        }
    }
}