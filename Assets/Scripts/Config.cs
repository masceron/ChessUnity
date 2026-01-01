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
        new PieceConfig("piece_archelon", true, 86),
    };

    public static RelicConfig relicWhiteConfig = new("relic_storm_capacitor", false, 5);
    public static RelicConfig relicBlackConfig = new("relic_sirens_harpoon", true, 5);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.None;
}