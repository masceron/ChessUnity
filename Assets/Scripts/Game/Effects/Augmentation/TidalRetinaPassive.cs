using Game.Piece.PieceLogic;
using UnityEngine;

namespace Game.Effects.Augmentation
{
    public class TidalRetinaPassive : Effect
    {
        public TidalRetinaPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectName.TidalRetinaPassive)
        { }
    }
}

