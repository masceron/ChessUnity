using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnipeEelPassive : Effect
    {
        public SnipeEelPassive(PieceLogic piece) : base(-1, -1, piece, EffectName.SnipeEelPassive)
        {
            
        }
    }
}