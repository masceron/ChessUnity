using MemoryPack;
using UnityEngine;

namespace Game.Save.Stage
{
    [MemoryPackable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public partial struct GameConfig
    {
        public readonly bool FirstSideToMove;
        public readonly bool OurSide;
        public readonly bool CanChoosePiece;
        public readonly Vector2Int StartingSize;

        public GameConfig(bool firstSideToMove, bool ourSide, Vector2Int startingSize, bool canChoosePiece = false)
        {
            CanChoosePiece = canChoosePiece;
            FirstSideToMove = firstSideToMove;
            OurSide = ourSide;
            StartingSize = startingSize;
        }
    }
}