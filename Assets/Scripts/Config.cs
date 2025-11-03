using System.Collections.Generic;
using Game.Piece;
using Game.Relics;

public static class Config
{
    public static readonly List<PieceConfig> PieceConfigWhite = new()
    {
        //new PieceConfig(PieceType.Velkaris, false, 140),
        //new PieceConfig(PieceType.SeaUrchin, false, 100),
        // new PieceConfig(PieceType.ElectricEel, false, 92),
        // new PieceConfig(PieceType.Chrysos, false, 88),
        // new PieceConfig(PieceType.Archelon, false, 78),
        // new PieceConfig(PieceType.Thalassos, false, 65),
        // new PieceConfig(PieceType.Pufferfish, false, 70),
        // new PieceConfig(PieceType.Lionfish, false, 99),
        // new PieceConfig(PieceType.Seahorse, false, 55),
        // new PieceConfig(PieceType.Remora, false, 91),
        new PieceConfig(PieceType.ArcticBrittleStar, false, 101),
         new PieceConfig(PieceType.PhantomJelly, false, 80),
        new PieceConfig(PieceType.Anomalocaris, false, 100),
        new PieceConfig(PieceType.GuidingSiren, false, 91)
    };

    public static readonly List<PieceConfig> PieceConfigBlack = new()
    {
         new PieceConfig(PieceType.ClownFish, true, 88),
         new PieceConfig(PieceType.GuidingSiren, true, 87),
         new PieceConfig(PieceType.PollutedRock, true, 89),
         new PieceConfig(PieceType.Barnacle, true, 90),
         //new PieceConfig(PieceType.Anomalocaris, true, 99),
         //new PieceConfig(PieceType.Swordfish, true, 101),
         //new PieceConfig(PieceType.ElectricEel, true, 112),
         //new PieceConfig(PieceType.ElectricEel, true, 111),
         //new PieceConfig(PieceType.SeaStar, true, 113),
         //new PieceConfig(PieceType.Swordfish, true, 10),
        // new PieceConfig(PieceType.Anglerfish, true, 93),
    };

    public static readonly RelicConfig relicWhiteConfig = new RelicConfig(RelicType.SirensHarpoon, false, 5);
    public static readonly RelicConfig relicBlackConfig = new RelicConfig(RelicType.SirensHarpoon, true, 5);
}