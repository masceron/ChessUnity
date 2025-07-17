using Game.Board.General;

namespace Game.Board.Piece
{
    public struct PieceConfig
    {
        public readonly PieceType Type;
        public readonly Color Color;
        public readonly ushort Index;

        public PieceConfig(PieceType t, Color c, ushort i)
        {
            Type = t;
            Color = c;
            Index = i;
        }
    }
}