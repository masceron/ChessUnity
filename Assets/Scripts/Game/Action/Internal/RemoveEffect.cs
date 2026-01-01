using Game.Common;
using Game.Effects;

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
            if (effect.Category == EffectCategory.Augmentation && effect.Duration < 0) return;
            BoardUtils.RemoveEffectObserver(effect);
            
            if (effect is IOnRemove onRemove)
                onRemove.OnRemove();
            effect.Piece.Effects.Remove(effect);
        }
    }
}