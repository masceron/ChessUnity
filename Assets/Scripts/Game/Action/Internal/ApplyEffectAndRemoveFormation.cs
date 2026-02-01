using Game.Action;
using Game.Managers;
using Game.Tile;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffectAndRemoveFormation : Action, IInternal
    {
        private readonly ApplyEffect applyEffectAction;
        private readonly int formationPos;

        public ApplyEffectAndRemoveFormation(ApplyEffect applyEffect, int formationPosition) : base(-1)
        {
            applyEffectAction = applyEffect;
            formationPos = formationPosition;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            if (applyEffectAction.Result == ResultFlag.Success)
            {
                FormationManager.Ins.RemoveFormation(formationPos);
            }
        }
    }
}
