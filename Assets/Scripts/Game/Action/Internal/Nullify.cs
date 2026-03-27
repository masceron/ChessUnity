using Game.Effects;
using ZLinq;

namespace Game.Action.Internal
{
    public class Nullify : Action, IInternal
    {
        public Nullify(int maker, int to) : base(maker, to)
        {
        }

        protected override void ModifyGameState()
        {
            foreach (var effect in GetTarget().Effects
                         .Where(effect => effect.Category == EffectCategory.Buff))
                ActionManager.EnqueueAction(new RemoveEffect(effect));
        }
    }
}