using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Broken: Effect
    {
        public Broken(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_broken")
        {}
        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }
    }
}