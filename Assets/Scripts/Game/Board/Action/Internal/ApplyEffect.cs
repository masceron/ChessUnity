using Game.Board.Effects;
using Game.Board.General;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ApplyEffect: Action
    {
        private readonly Effect effect;
        public ApplyEffect(Effect e) : base(-1, false, false, true)
        {
            effect = e;
        }

        public override void ApplyAction(GameState state)
        {
            if (effect.Type != ObserverType.None)
            {
                EventObserver.AddObserver(effect);
            }
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            effect.OnApply();
            effect.Piece.Effects.Add(effect);
        }
    }
}