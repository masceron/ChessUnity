using Game.Piece.PieceLogic;
using UnityEngine;

namespace Game.Effects.Augmentation
{
    public class TidalRetinaPassive : Effect, IMoveRangeModifier
    {
        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + Strength;
        }
        public TidalRetinaPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectName.TidalRetinaPassive)
        { }
    }
}

