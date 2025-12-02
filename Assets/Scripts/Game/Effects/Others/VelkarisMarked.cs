using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMarked: Effect
    {
        public VelkarisMarked(PieceLogic piece) : base(-1, 1, piece, "effect_velkaris_marked")
        {}
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80;
        }
    }
}