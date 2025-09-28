using Game.Piece;
using MemoryPack;

namespace Game.Save.Stage
{
    [MemoryPackable]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public partial struct LineupConfig
    {
        public readonly PieceConfig[] WhiteConfig;
        public readonly PieceConfig[] BlackConfig;

        public LineupConfig(PieceConfig[] whiteConfig, PieceConfig[] blackConfig)
        {
            WhiteConfig = whiteConfig;
            BlackConfig = blackConfig;
        }
    }
}