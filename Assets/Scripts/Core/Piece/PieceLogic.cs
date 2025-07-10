using System.Collections.Generic;
using Board.Action;
using Core.General;

namespace Core.Piece
{
    public abstract class PieceLogic
    {
        public ushort Pos;
        public readonly PieceType Type;
        public Color Color;
        protected readonly sbyte Range;
        public PieceRank Rank;
       
        public readonly List<Effect.Effect> Effects;
        public readonly List<TriggerElement> Triggers = new();

        protected PieceLogic(PieceType type, Color color, ushort pos, List<Effect.Effect> effects, sbyte r)
        {
            Type = type;
            Color = color;
            Pos = pos;
            Effects = effects;
            Range = r;
        }

        public abstract void PassTurn();
        public abstract List<Action> MoveToMake();
    }
}