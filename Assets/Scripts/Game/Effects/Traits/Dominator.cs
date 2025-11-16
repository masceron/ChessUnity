using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Dominator: Effect
    {
        public Dominator(PieceLogic piece) : base(-1, 1, piece, "effect_dominator")
        {}
    }
}