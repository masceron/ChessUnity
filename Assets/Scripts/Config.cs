using System.Collections.Generic;
using Game.Board.General;
using Game.Board.Piece;

public class Config
{
    public readonly List<PieceConfig> PieceConfig = new()
    {
        new PieceConfig(PieceType.Velkaris, Color.White, 140),
        new PieceConfig(PieceType.Barracuda, Color.Black, 113),
        new PieceConfig(PieceType.GuidingSiren, Color.Black, 101),
        new PieceConfig(PieceType.SeaUrchin, Color.White, 100),
        new PieceConfig(PieceType.ElectricEel, Color.White, 92),
        new PieceConfig(PieceType.FlyingFish, Color.Black, 112),
        new PieceConfig(PieceType.Chrysos, Color.White, 88),
        new PieceConfig(PieceType.Anomalocaris, Color.Black, 75),
        new PieceConfig(PieceType.Archelon, Color.White, 78)
    };
        
    public readonly byte[] BoardActive = {
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    };

    public readonly Color OurSide = Color.White;

    public readonly Color SideToMove = Color.White;

    public Config()
    {
            
    }

    public Config(List<PieceConfig> pieces, byte[] ac, Color ourSide, Color sideToMove)
    {
        PieceConfig = pieces;
        BoardActive = ac;
        OurSide = ourSide;
        SideToMove = sideToMove;
    }
}