using Game.Board.Effects;
using Game.Board.General;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RemoveEffect: Action, IInternal
    {
        private readonly Effect effect;
        
        public RemoveEffect(Effect e) : base(-1, false)
        {
            effect = e;
        }

        protected override void ModifyGameState()
        {
            if (effect.ObserverType != ObserverType.None)
            {
                EventObserver.RemoveObserver(effect);
            }
            effect.OnRemove();
            effect.Piece.Effects.Remove(effect);
        }
    }
}