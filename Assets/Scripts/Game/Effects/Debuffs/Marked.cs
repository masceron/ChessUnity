using Game.Piece.PieceLogic;
using UnityEngine;

namespace Game.Effects.Debuffs
{
    public class Marked : Effect
    {
        public Marked(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Marked)
        {

        }
    }
}

