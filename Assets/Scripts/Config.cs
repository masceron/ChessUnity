using System.Collections.Generic;
using Game.Piece;
using Game.Relics;
using UnityEngine.TextCore;

public static class Config
{
    public static readonly List<PieceConfig> PieceConfigWhite = new()
    {
        // new PieceConfig(PieceType.Velkaris, false, 140),
        // new PieceConfig(PieceType.SeaUrchin, false, 100),
        // new PieceConfig(PieceType.ElectricEel, false, 92),
        // new PieceConfig(PieceType.Chrysos, false, 88),
        // new PieceConfig(PieceType.Archelon, false, 78),
        // new PieceConfig(PieceType.Thalassos, false, 65),
        // new PieceConfig(PieceType.Pufferfish, false, 70),
        // new PieceConfig(PieceType.Lionfish, false, 99),
        // new PieceConfig(PieceType.Seahorse, false, 55),
        // new PieceConfig(PieceType.Remora, false, 91),
        // new PieceConfig(PieceType.MedicalLeech, false, 1),
        new PieceConfig(PieceType.Megalodon, false, 93),
        new PieceConfig(PieceType.GuidingSiren, false, 91)
    };

    public static readonly List<PieceConfig> PieceConfigBlack = new()
    {
         new PieceConfig(PieceType.Barracuda, true, 66),
        // new PieceConfig(PieceType.GuidingSiren, true, 101),
        // new PieceConfig(PieceType.FlyingFish, true, 112),
        // new PieceConfig(PieceType.Anomalocaris, true, 75),

        // new PieceConfig(PieceType.Swordfish, true, 98),
        new PieceConfig(PieceType.MorayEel, true, 69),
        // new PieceConfig(PieceType.Stingray, true, 96),
        // new PieceConfig(PieceType.SeaStar, true, 97),
        // new PieceConfig(PieceType.SeaStar, true, 90),
        // new PieceConfig(PieceType.Anglerfish, true, 93),
        new PieceConfig(PieceType.StoneGrab, true, 115),
        new PieceConfig(PieceType.StoneGrab, true, 118),
         new PieceConfig(PieceType.Humilitas, true, 93),
         new PieceConfig(PieceType.Thalassos, true, 94),
    };

    public static readonly RelicConfig relicWhiteConfig = new RelicConfig(RelicType.FrostSigil, false, 5);
    public static readonly RelicConfig relicBlackConfig = new RelicConfig(RelicType.EyeOfMimic, true, 5);
}