using Game.Board.Effects;
using Game.Board.General;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffect: Action, IInternal
    {
        private readonly Effect effect;
        public ApplyEffect(Effect e) : base(-1, false)
        {
            effect = e;
        }

        protected override void Animate()
        {
            if (effect.ObserverType != ObserverType.None)
            {
                EventObserver.AddObserver(effect);
            }
        }

        protected override void ModifyGameState()
        {
            effect.OnApply();
            effect.Piece.Effects.Add(effect);
        }
    }
}