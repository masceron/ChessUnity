using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Controlled: Effect, IApplyEffect
    {
        private readonly bool initSide;
        public Controlled(sbyte duration, PieceLogic piece) : base(duration, -1, piece, "effect_controlled")
        {
            initSide = piece.Color;
        }

        // public override void OnCall(Action.Action action)
        // {
        //     Piece.Color = !_initSide;
        // }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            Piece.Color = !initSide;
        }

        public override void OnRemove()
        {
            Piece.Color = initSide;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}