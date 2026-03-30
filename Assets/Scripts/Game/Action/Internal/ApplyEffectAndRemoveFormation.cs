using Game.Tile;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffectAndRemoveFormation : Action, IInternal
    {
        private readonly ApplyEffect _applyEffectAction;

        public ApplyEffectAndRemoveFormation(ApplyEffect applyEffect, Formation formation) : base(null, formation)
        {
            _applyEffectAction = applyEffect;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            if (_applyEffectAction.Result == ResultFlag.Success) RemoveFormation(GetTargetAsFormation());
        }
    }
}