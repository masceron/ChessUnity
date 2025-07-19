using System.Collections.Generic;
using Game.Board.General;
using Game.Board.Piece;

public class Config
{
    public List<PieceConfig> PieceConfig = new()
    {
        new PieceConfig(PieceType.Velkaris, Color.White, 140),
        new PieceConfig(PieceType.Barracuda, Color.Black, 104),
        new PieceConfig(PieceType.GuidingSiren, Color.Black, 101),
        new PieceConfig(PieceType.SeaUrchin, Color.White, 100),
        new PieceConfig(PieceType.ElectricEel, Color.Black, 92),
        new PieceConfig(PieceType.FlyingFish, Color.White, 112)
    };
        
    public byte[] BoardActive = {
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

    public Color OurSide = Color.White;

    public Color SideToMove = Color.White;

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