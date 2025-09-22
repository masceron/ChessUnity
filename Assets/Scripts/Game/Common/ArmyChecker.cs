using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Piece;

namespace Game.Common
{
    public static class ArmyChecker
    {
        public static readonly ReadOnlyDictionary<PieceRank, int> MaxOf = new(
            new Dictionary<PieceRank, int>
            {
                [PieceRank.Commander] = 1,
                [PieceRank.Champion] = 2,
                [PieceRank.Elite] = 4,
                [PieceRank.Common] = 10,
                [PieceRank.Swarm] = int.MaxValue
            }
        );
    }
}