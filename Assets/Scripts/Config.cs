using System.Collections.Generic;
using Game.Augmentation;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics;

public static class Config
{
    public static int boardSize = 12;
    public static readonly List<PieceConfig> PieceConfigWhite = new()
    {
        new PieceConfig(PieceType.BobtailSquid, false, 35),
        new PieceConfig(PieceType.Lizardfish, false, 77, new List<Augmentation>{new TidalRetina()}),
    };

    public static readonly List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig(PieceType.Temperantia, true, 20),
        new PieceConfig(PieceType.HourglassJelly, true, 30),
        new PieceConfig(PieceType.Swordfish, true, 10),

    };

    public static RelicConfig relicWhiteConfig = new RelicConfig(RelicType.BlackPearl, false, 5);
    public static RelicConfig relicBlackConfig = new RelicConfig(RelicType.BlackPearl, true, 5);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.Whirpool;
}