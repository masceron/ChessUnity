using System.Collections.Generic;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics.Commons;

public static class Config
{
    public static int boardSize = 12;
    public static List<PieceConfig> PieceConfigWhite = new()
    {
        // new PieceConfig("piece_siren", false, 25),
        // new PieceConfig("piece_bobtail_squid", false, 26),
        // new PieceConfig("piece_hatchetfish", false, 21),
        // new PieceConfig("piece_blue_dragon", false, 22),
        // new PieceConfig("piece_umbrella_slug", false, 23),
    };

    public static List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig("piece_stingray", true, 16),
        new PieceConfig("piece_cutthroat_eel", true, 17),
        new PieceConfig("piece_archelon", true, 18),
        // new PieceConfig("piece_eyeshade_sculpin", true, 18),
    };

    public static RelicConfig relicWhiteConfig = new RelicConfig("relic_storm_capacitor", false, 5);
    public static RelicConfig relicBlackConfig = new RelicConfig("relic_sirens_harpoon", true, 5);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.None;
}