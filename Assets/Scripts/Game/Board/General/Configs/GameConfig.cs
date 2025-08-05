using System;
using UnityEngine;

namespace Game.Board.General.Configs
{
    [Serializable]
    public struct GameConfig
    {
        public readonly bool FirstSideToMove;
        public readonly bool OurSide;
        public readonly Vector2Int StartingSize;

        public GameConfig(bool firstSideToMove, bool ourSide, Vector2Int startingSize)
        {
            FirstSideToMove = firstSideToMove;
            OurSide = ourSide;
            StartingSize = startingSize;
        }
    }
}