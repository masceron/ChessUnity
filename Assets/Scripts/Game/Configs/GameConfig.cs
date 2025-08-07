using UnityEngine;

namespace Game.Configs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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