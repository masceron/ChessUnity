using Game.Board.Piece.PieceLogic;
using System;
using Game.Board.General;

namespace Game.Board.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Slow: Effect
    {
        public Slow(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectType.Slow)
        {}

        public override void OnApply()
        {
            Piece.TrueMoveRange -= Strength;
            
            if (Piece.EffectiveMoveRange > 0) Piece.EffectiveMoveRange = Math.Max(Piece.TrueMoveRange, (sbyte)1);
        }

        public override void OnRemove()
        {
            Piece.TrueMoveRange += Strength;
            
            if (Piece.EffectiveMoveRange > 0) Piece.EffectiveMoveRange = Math.Max(Piece.TrueMoveRange, (sbyte)1);
        }

        public override string Description()
        {
            return string.Format(MatchManager.assetManager.EffectData[EffectName].description, Strength);
        }
    }
}