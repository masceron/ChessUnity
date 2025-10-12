using Game.Piece.PieceLogic;
using Game.Action;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class QuickReflex: Effect
    {
        public QuickReflex(PieceLogic piece) : base(-1, 1, piece, EffectName.QuickReflex)
        {}
        
    }
}