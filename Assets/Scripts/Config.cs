using System.Collections.Generic;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics;

public static class Config
{
    public static int boardSize = 12;
    public static List<PieceConfig> PieceConfigWhite = new()
    {
        new PieceConfig("piece_anglerfish", false, 80),
        new PieceConfig("piece_humilitas", false, 89),
        new PieceConfig("piece_megalodon", false, 76),
        new PieceConfig("piece_horseleech", false, 84)
    };

    public static List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig("piece_swordfish", true, 85),
        new PieceConfig("piece_siren", true, 88),
        new PieceConfig("piece_archerfish", true, 87)
    };

    public static RelicConfig relicWhiteConfig = new RelicConfig(RelicType.SirensHarpoon, false, 5);
    public static RelicConfig relicBlackConfig = new RelicConfig(RelicType.SirensHarpoon, true, 5);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.Whirpool;
}