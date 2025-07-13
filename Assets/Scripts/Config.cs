using System.Collections.Generic;
using Game.Board.General;
using Game.Board.Piece;

public class Config
{
    public int MaxRank = 12;
    public int MaxFile = 12;
        
    public List<PieceConfig> PieceConfig = new()
    {
        new PieceConfig(PieceType.Velkaris, Color.White, 140),
        new PieceConfig(PieceType.Barracuda, Color.Black, 104),
        new PieceConfig(PieceType.GuidingSiren, Color.Black, 101)
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

    public Config(int r, int f, List<PieceConfig> pieces, byte[] ac, Color ourSide, Color sideToMove)
    {
        MaxRank = r;
        MaxFile = f;
        PieceConfig = pieces;
        BoardActive = ac;
        OurSide = ourSide;
        SideToMove = sideToMove;
    }
}