using System.Collections.Generic;
using Game.Board.Piece;

public class Config
{
    public readonly List<PieceConfig> PieceConfig = new()
    {
        new PieceConfig(PieceType.Velkaris, false, 140),
        new PieceConfig(PieceType.Barracuda, true, 113),
        new PieceConfig(PieceType.GuidingSiren, true, 101),
        new PieceConfig(PieceType.SeaUrchin, false, 100),
        new PieceConfig(PieceType.ElectricEel, false, 92),
        new PieceConfig(PieceType.FlyingFish, true, 112),
        new PieceConfig(PieceType.Chrysos, false, 88),
        new PieceConfig(PieceType.Anomalocaris, true, 75),
        new PieceConfig(PieceType.Archelon, false, 78),
        new PieceConfig(PieceType.Thalassos, true, 65),
        new PieceConfig(PieceType.Pufferfish, false, 70),
        new PieceConfig(PieceType.Swordfish, true, 98),
        new PieceConfig(PieceType.Lionfish, false, 99),
        new PieceConfig(PieceType.MorayEel, false, 95),
        new PieceConfig(PieceType.Stingray, true, 96),
        new PieceConfig(PieceType.Seahorse, false, 55),
        new PieceConfig(PieceType.SeaStar, false, 97),
        new PieceConfig(PieceType.SeaStar, false, 90)
    };
}