using Game.Common;
using Game.Effects;
using Game.Effects.Triggers;
using UnityEngine;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RemoveEffect : Action, IInternal
    {
        private readonly Effect _effect;

        public RemoveEffect(Effect e) : base(-1)
        {
            _effect = e;
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Removing " + _effect.GetType() + ": " + _effect.Duration);
            BoardUtils.RemoveObserver(_effect);

            if (_effect is IOnRemoveTrigger onRemove)
                onRemove.OnRemove();
            _effect.Piece.Effects.Remove(_effect);
        }
    }
}