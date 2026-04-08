using System.Collections.Generic;
using Game.Effects.FieldEffect;
using Game.Piece;
using Game.Relics.Commons;

public static class Config
{
    public static int BoardSize = 12;

    public static List<PieceConfig> PieceConfigWhite = new()
    {
        new PieceConfig("piece_chrysos", false, 111),
        new PieceConfig("piece_redtail_parrotfish", false, 98),
        new PieceConfig("piece_swordfish", false, 88),
    };

    public static List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig("piece_temperantia", true, 20),
        new PieceConfig("piece_archelon", true, 25)
    };

    public static RelicConfig relicWhiteConfig = new("relic_coral_tome", false, 6);
    public static RelicConfig relicBlackConfig = new("relic_coral_tome", true, 6);
    public static FieldEffectType FieldEffectType = FieldEffectType.None;

    public static void SetBlackPieceConfig(List<PieceConfig> pieceConfigs)
    {
        if (pieceConfigs != null) PieceConfigBlack = pieceConfigs;
    }
}