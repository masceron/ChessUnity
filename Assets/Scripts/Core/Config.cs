using System.Collections.Generic;

namespace Core
{
    public struct PieceConfig
    {
        public PieceType Type;
        public Color Color;
        public byte Index;

        public PieceConfig(PieceType t, Color c, byte i)
        {
            Type = t;
            Color = c;
            Index = i;
        }
    }
    public static class Config
    {
        public static readonly List<PieceConfig> pieceConfig = new()
        {
            new PieceConfig(PieceType.Velkaris, Color.White, 140),
           // new PieceConfig(PieceType.Velkaris, Color.White, 26),
            new PieceConfig(PieceType.Velkaris, Color.Black, 101)
        };
        
        public static readonly byte[] boardActive = {
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
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
    }
}