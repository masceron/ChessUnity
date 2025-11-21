using System.Collections.Generic;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics.Commons;

public static class Config
{
    public static int boardSize = 12;
    public static List<PieceConfig> PieceConfigWhite = new()
    {
        new PieceConfig("piece_anglerfish", false, 80),
    };

    public static List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig("piece_swordfish", true, 85)
    };

    public static RelicConfig relicWhiteConfig = new RelicConfig("relic_siren_harpoon", false, 5);
    public static RelicConfig relicBlackConfig = new RelicConfig("relic_siren_harpoon", true, 5);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.Whirpool;
}