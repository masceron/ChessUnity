using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Controlled: Effect, IOnApply, IOnRemove
    {
        private readonly bool initSide;
        public Controlled(int duration, PieceLogic piece) : base(duration, -1, piece, "effect_controlled")
        {
            initSide = piece.Color;
        }

        public void OnApply()
        {
            Piece.Color = !initSide;
        }

        public void OnRemove()
        {
            Piece.Color = initSide;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}