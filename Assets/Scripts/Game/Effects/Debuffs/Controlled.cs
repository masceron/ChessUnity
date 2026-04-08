using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Controlled : Effect, IOnApplyTrigger, IOnRemoveTrigger
    {
        public Controlled(int duration, PieceLogic piece) : base(duration, -1, piece, "effect_controlled")
        {
        }

        public void OnApply()
        {
            Piece.Color = !Piece.Color;
        }

        public void OnRemove()
        {
            Piece.Color = !Piece.Color;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}