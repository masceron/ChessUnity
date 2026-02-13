using System;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Triggers
{
    public interface IOnPieceSpawnedTrigger : IComparable<IOnPieceSpawnedTrigger>
    {
        int IComparable<IOnPieceSpawnedTrigger>.CompareTo(IOnPieceSpawnedTrigger other)
        {
            return 0;
        }

        public void OnPieceSpawn(PieceLogic piece);
    }
}