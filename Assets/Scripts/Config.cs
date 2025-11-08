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
        // new PieceConfig(PieceType.Velkaris, false, 140),
        //new PieceConfig(PieceType.SeaUrchin, false, 100),
        // new PieceConfig(PieceType.ElectricEel, false, 92),
        // new PieceConfig(PieceType.Chrysos, false, 88),
        // new PieceConfig(PieceType.Archelon, false, 78),
        // new PieceConfig(PieceType.Thalassos, false, 65),
        // new PieceConfig(PieceType.Pufferfish, false, 70),
        // new PieceConfig(PieceType.Lionfish, false, 99),
        new PieceConfig(PieceType.Seahorse, false, 0),
        // new PieceConfig(PieceType.Remora, false, 91),
         new PieceConfig(PieceType.Lizardfish, false, 77, new List<Augmentation>{new TidalRetina()}),
        //new PieceConfig(PieceType.BioluminescentBeacon, false, 100),
        //new PieceConfig(PieceType.GuidingSiren, false, 91)
    };

    public static readonly List<PieceConfig> PieceConfigBlack = new()
    {
        //  new PieceConfig(PieceType.PhantomJelly, true, 101),
        new PieceConfig(PieceType.GuidingSiren, true, 87),
         //new PieceConfig(PieceType.FlyingFish, true, 89),
         //new PieceConfig(PieceType.Anomalocaris, true, 99),
         //new PieceConfig(PieceType.Swordfish, true, 101),
         //new PieceConfig(PieceType.ElectricEel, true, 112),
         //new PieceConfig(PieceType.ElectricEel, true, 111),
         //new PieceConfig(PieceType.SeaStar, true, 113),
         //new PieceConfig(PieceType.Swordfish, true, 10),
        // new PieceConfig(PieceType.Anglerfish, true, 93),
    };

    public static RelicConfig relicWhiteConfig = new RelicConfig(RelicType.BlackPearl, false, 5);
    public static RelicConfig relicBlackConfig = new RelicConfig(RelicType.BlackPearl, true, 5);
    public static RegionalEffectType regionalEffectType = RegionalEffectType.Whirpool;
}