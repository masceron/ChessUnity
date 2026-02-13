using System.Collections.Generic;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics.Commons;

public static class Config
{
    public static int boardSize = 12;
    public static List<PieceConfig> PieceConfigWhite = new()
    {
        new PieceConfig("piece_redtail_parrotfish", false, 111),
        new PieceConfig("piece_redtail_parrotfish", false, 98),
    };

    public static List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig("piece_blue_dragon", true, 20),
        new PieceConfig("piece_archelon", true, 25),
    };

    public static RelicConfig relicWhiteConfig = new("relic_coral_tome", false, 6);
    public static RelicConfig relicBlackConfig = new("relic_coral_tome", true, 6);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.None;

    public static void SetBlackPieceConfig(List<PieceConfig> pieceConfigs)
    {
        if (pieceConfigs != null)
        {
            PieceConfigBlack = pieceConfigs;
        }
    }
}