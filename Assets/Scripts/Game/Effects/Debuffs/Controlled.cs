using Game.Action.Internal;
using Game.Piece.PieceLogic;
using UnityEngine;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Controlled: Effect, IApplyEffect
    {
        private readonly bool _initSide;
        public Controlled(sbyte duration, PieceLogic piece) : base(duration, -1, piece, EffectName.Controlled)
        {
            _initSide = piece.Color;
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            Piece.Color = !_initSide;
            Debug.Log("called");
        }

        public override void OnRemove()
        {
            Piece.Color = _initSide;
        }
    }
}