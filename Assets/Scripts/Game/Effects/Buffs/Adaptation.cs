using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Adaptation : Effect, IOnApplyTrigger, IOnRemoveTrigger
    {
        public Adaptation(PieceLogic piece) : base(-1, 1, piece, "effect_adaptation")
        {
        }

        public void OnApply()
        {
            Piece.AddImmunity(ImmunityType.FormationDebuff);
        }

        public void OnRemove()
        {
            Piece.RemoveImmunity(ImmunityType.FormationDebuff);
        }
    }
}