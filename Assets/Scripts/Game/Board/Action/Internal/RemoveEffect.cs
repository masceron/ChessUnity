using Game.Board.Effects;
using Game.Board.General;
using Game.Common;

namespace Game.Board.Action.Internal
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
            if (effect.ObserverActivateWhen != ObserverActivateWhen.None)
            {
                BoardUtils.RemoveObserver(effect);
            }
            effect.OnRemove();
            effect.Piece.Effects.Remove(effect);
        }
    }
}