using System.Collections.Generic;
using Game.Piece;

public static class Config
{
    public static readonly List<PieceConfig> PieceConfigWhite = new()
    {
        // new PieceConfig(PieceType.Pufferfish, false, 70),
        // new PieceConfig(PieceType.Lionfish, false, 99),
        // new PieceConfig(PieceType.Seahorse, false, 55),
        // new PieceConfig(PieceType.Remora, false, 91),
        // new PieceConfig(PieceType.MedicalLeech, false, 1),
        new PieceConfig(PieceType.HourglassJelly, false, 17),
        new PieceConfig(PieceType.Temperantia, false, 50)
};

    public static readonly List<PieceConfig> PieceConfigBlack = new()
    {
        // new PieceConfig(PieceType.Barracuda, true, 113),
        // new PieceConfig(PieceType.GuidingSiren, true, 101),
        // new PieceConfig(PieceType.FlyingFish, true, 112),
        // new PieceConfig(PieceType.Anomalocaris, true, 75),
        // new PieceConfig(PieceType.Swordfish, true, 98),
        // new PieceConfig(PieceType.MorayEel, true, 95),
        // new PieceConfig(PieceType.Stingray, true, 96),
        // new PieceConfig(PieceType.SeaStar, true, 97),
        // new PieceConfig(PieceType.SeaStar, true, 90),
        new PieceConfig(PieceType.Anglerfish, true, 93),
        new PieceConfig(PieceType.BobtailSquid, true, 55),
    };
}