using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Broken : Effect, IOnApplyTrigger, IOnRemoveTrigger
    {
        private Effect specialAbility;

        public Broken(int duration, PieceLogic piece) : base(duration, 1, piece, "effect_broken")
        {
        }

        public void OnApply()
        {
            specialAbility = Piece.Effects.FirstOrDefault(e => e.Category == EffectCategory.SpecialAbility);
            if (specialAbility != null) specialAbility.disabled = true;
        }

        public void OnRemove()
        {
            if (specialAbility != null) specialAbility.disabled = false;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }
    }
}