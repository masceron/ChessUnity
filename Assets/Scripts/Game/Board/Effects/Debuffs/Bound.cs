using System;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Bound: Effect
    {
        public Bound(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Bound)
        {}

        public override void OnApply()
        {
            Piece.EffectiveMoveRange = 0;
        }

        public override void OnRemove()
        {
            Piece.EffectiveMoveRange = Math.Max(Piece.TrueMoveRange, (sbyte)1);
        }
    }
}