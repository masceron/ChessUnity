using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnipeEelPassive : Effect
    {
        public SnipeEelPassive(PieceLogic piece) : base(-1, -1, piece, "effect_snipe_eel_passive")
        {
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }
    }
}