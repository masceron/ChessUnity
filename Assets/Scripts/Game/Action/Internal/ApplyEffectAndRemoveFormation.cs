using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffectAndRemoveFormation : Action, IInternal
    {
        private readonly ApplyEffect _applyEffectAction;
        private readonly int _formationPos;

        public ApplyEffectAndRemoveFormation(ApplyEffect applyEffect, int formationPosition) : base(-1)
        {
            _applyEffectAction = applyEffect;
            _formationPos = formationPosition;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            if (_applyEffectAction.Result == ResultFlag.Success) RemoveFormation(_formationPos);
        }
    }
}