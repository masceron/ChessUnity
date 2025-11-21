using System.Collections.Generic;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics;

public static class Config
{
    public static int boardSize = 12;
    public static List<PieceConfig> PieceConfigWhite = new()
    {
        new PieceConfig("piece_bobtail_squid", false, 80),
        new PieceConfig("piece_temperantia", false, 70)
    };

    public static List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig("piece_swordfish", true, 85),
        new PieceConfig("piece_swordfish", true, 90),
        new PieceConfig("piece_bobtail_squid", true, 100),
    };

    public static RelicConfig relicWhiteConfig = new RelicConfig(RelicType.SirensHarpoon, false, 5);
    public static RelicConfig relicBlackConfig = new RelicConfig(RelicType.SirensHarpoon, true, 5);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.None;
}