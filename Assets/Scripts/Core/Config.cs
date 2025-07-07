using System.Collections.Generic;

namespace Core
{
    public struct PieceConfig
    {
        public readonly PieceType Type;
        public readonly Color Color;
        public readonly ushort Index;
        public List<Effect> Effects;

        public PieceConfig(PieceType t, Color c, ushort i, List<Effect> e)
        {
            Type = t;
            Color = c;
            Index = i;
            Effects = e;
        }
    }
    public static class Config
    {
        public static readonly List<PieceConfig> PieceConfig = new()
        {
            new PieceConfig(PieceType.Velkaris, Color.White, 140, new List<Effect>()),
           // new PieceConfig(PieceType.Velkaris, Color.White, 26),
            new PieceConfig(PieceType.GuidingSiren, Color.Black, 101, new List<Effect>())
        };
        
        public static readonly byte[] BoardActive = {
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
    }
}