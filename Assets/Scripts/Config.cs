using System.Collections.Generic;
using Game.Piece;

public static class Config
{
    public static readonly List<PieceConfig> PieceConfigWhite = new()
    {

        new PieceConfig(PieceType.Chrysos, false, 88),
        new PieceConfig(PieceType.Archelon, false, 78),
        new PieceConfig(PieceType.Thalassos, false, 65),
        new PieceConfig(PieceType.Pufferfish, false, 70),
        new PieceConfig(PieceType.Lionfish, false, 99),
        new PieceConfig(PieceType.Seahorse, false, 55),
        new PieceConfig(PieceType.Remora, false, 91),
        new PieceConfig(PieceType.MedicalLeech, false, 1),
        new PieceConfig(PieceType.HermitCrab, false, 60),
        new PieceConfig(PieceType.HermitCrab, false, 66)

    };

    public static readonly List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig(PieceType.Swordfish, true, 98),
        new PieceConfig(PieceType.MorayEel, true, 95),
        new PieceConfig(PieceType.Stingray, true, 96),
        new PieceConfig(PieceType.SeaStar, true, 97),
        new PieceConfig(PieceType.SeaStar, true, 90),
        new PieceConfig(PieceType.Anglerfish, true, 93),
        new PieceConfig(PieceType.HermitCrab, true, 50),
        new PieceConfig(PieceType.SeaTurtle, true, 51),
        new PieceConfig(PieceType.Pufferfish, true, 52)
    };
}