using System.Collections.Generic;
using UnityEngine;

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
            new PieceConfig(PieceType.Velkaris, Color.White, 25),
            new PieceConfig(PieceType.Velkaris, Color.White, 52),
            new PieceConfig(PieceType.Velkaris, Color.Black, 53),
            new PieceConfig(PieceType.Velkaris, Color.Black, 65),
        };
        
        
        
        public static readonly byte[] boardActive = {
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
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };
    }
}