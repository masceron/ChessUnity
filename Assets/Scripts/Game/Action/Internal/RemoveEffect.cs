using Game.Common;
using Game.Effects;
using Game.Managers;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RemoveEffect: Action, IInternal
    {
        private readonly Effect effect;
        
        public RemoveEffect(Effect e) : base(-1)
        {
            effect = e;
        }

        protected override void ModifyGameState()
        {
            if (effect == null) { return; }
            if (effect.Category == EffectCategory.Augmentation && effect.Duration < 0) return;
            if (effect.ObserverActivateWhen != ObserverActivateWhen.None)
            {
                BoardUtils.RemoveObserver(effect);
            }
            effect.OnRemove();
            effect.Piece.Effects.Remove(effect);
        }
    }
}