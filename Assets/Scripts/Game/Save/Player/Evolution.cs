using Game.Piece;
using MemoryPack;

namespace Game.Save.Player
{
    [MemoryPackable]
    public partial struct Evolution
    {
        private PieceType piece;
    }
}