using System;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Triggers
{
    public interface IOnPieceSpawnedTrigger: IComparable<IOnPieceSpawnedTrigger>
    {
        public void OnPieceSpawn(PieceLogic piece);

        int IComparable<IOnPieceSpawnedTrigger>.CompareTo(IOnPieceSpawnedTrigger other)
        {
            return 0;
        }
    }
}