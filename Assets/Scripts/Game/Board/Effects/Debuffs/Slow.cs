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
            Piece.trueMoveRange -= Strength;
            
            if (Piece.effectiveMoveRange > 0) Piece.effectiveMoveRange = Math.Max(Piece.trueMoveRange, (sbyte)1);
        }

        public override void OnRemove()
        {
            Piece.trueMoveRange += Strength;
            
            if (Piece.effectiveMoveRange > 0) Piece.effectiveMoveRange = Math.Max(Piece.trueMoveRange, (sbyte)1);
        }

        public override string Description()
        {
            return string.Format(MatchManager.assetManager.EffectData[EffectName].description, Strength);
        }
    }
}