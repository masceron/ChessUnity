using System.Collections.Generic;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics;

public static class Config
{
    public static readonly List<PieceConfig> PieceConfigWhite = new()
    {
        new PieceConfig("piece_anglerfish", false, 80),
    };

    public static readonly List<PieceConfig> PieceConfigBlack = new()
    {
    };

    public static readonly RelicConfig relicWhiteConfig = new RelicConfig(RelicType.SirensHarpoon, false, 5);
    public static readonly RelicConfig relicBlackConfig = new RelicConfig(RelicType.SirensHarpoon, true, 5);
    public static readonly RegionalEffectType regionalEffectType = RegionalEffectType.Whirpool;
}