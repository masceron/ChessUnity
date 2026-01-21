using System.Collections.Generic;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics.Commons;

public static class Config
{
    public static int boardSize = 12;
    public static List<PieceConfig> PieceConfigWhite = new()
    {
        new PieceConfig("piece_dormant_fossil", false, 70),
        new PieceConfig("piece_dormant_fossil", false, 71),
    };

    public static List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig("piece_siren", true, 110),
        new PieceConfig("piece_archelon", true, 98),
        new PieceConfig("piece_dormant_fossil", true, 100),
    };

    public static RelicConfig relicWhiteConfig = new("relic_seabed_leveler", false, 6);
    public static RelicConfig relicBlackConfig = new("relic_seabed_leveler", true, 6);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.None;
}