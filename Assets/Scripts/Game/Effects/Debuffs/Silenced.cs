using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Silenced : Effect
    {
        public Silenced(PieceLogic piece) : base(-1, 1, piece, "effect_silenced")
        {
            
        }
    }
}