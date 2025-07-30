using System.Collections.Generic;
using Game.Board.Piece;
using UnityEngine;
using Color = Game.Board.General.Color;

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
        new PieceConfig(PieceType.Archelon, Color.White, 78),
        new PieceConfig(PieceType.Thalassos, Color.Black, 65)
    };

    public readonly Vector2Int StartingSize = new (12, 12);

    public readonly Color OurSide = Color.White;

    public readonly Color SideToMove = Color.White;

    public Config()
    {
        
    }

    public Config(List<PieceConfig> pieces, Vector2Int startingSize, Color ourSide, Color sideToMove)
    {
        PieceConfig = pieces;
        OurSide = ourSide;
        StartingSize = startingSize;
        SideToMove = sideToMove;
    }
}