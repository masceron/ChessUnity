using Game.Effects.FieldEffect;
using Game.Piece;
using Game.Relics.Commons;
using MemoryPack;

namespace Game.Save.Stage
{
    [MemoryPackable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public partial struct LineupConfig
    {
        public readonly RelicConfig WhiteRelic;
        public readonly RelicConfig BlackRelic;
        public readonly FieldEffectType FieldEffect;

        public readonly PieceConfig[] WhiteConfig;
        public readonly PieceConfig[] BlackConfig;

        public LineupConfig(PieceConfig[] whiteConfig, PieceConfig[] blackConfig, RelicConfig whiteRelic,
            RelicConfig blackRelic,
            FieldEffectType fieldEffect)
        {
            WhiteConfig = whiteConfig;
            BlackConfig = blackConfig;
            WhiteRelic = whiteRelic;
            BlackRelic = blackRelic;
            FieldEffect = fieldEffect;
        }
    }
}