using System.Collections.Generic;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics.Commons;

public static class Config
{
    public static int boardSize = 12;
    public static List<PieceConfig> PieceConfigWhite = new()
    {
        
    };

    public static List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig("piece_siren", true, 110),
        new PieceConfig("piece_archelon", true, 98),
    };

    public static RelicConfig relicWhiteConfig = new("relic_coral_tome", false, 6);
    public static RelicConfig relicBlackConfig = new("relic_coral_tome", true, 6);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.None;
}