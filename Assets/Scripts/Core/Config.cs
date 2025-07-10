using System.Collections.Generic;
using Core.General;

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
    public class Config
    {
        public int MaxRank = 12;
        public int MaxFile = 12;
        
        public List<PieceConfig> PieceConfig = new()
        {
            new PieceConfig(PieceType.Velkaris, Color.White, 140, new List<Effect>()),
            new PieceConfig(PieceType.GuidingSiren, Color.Black, 101, new List<Effect>())
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
}