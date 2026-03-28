using Game.Common;
using Game.Effects;
using ZLinq;

namespace Game.Action.Internal
{
    public class Purify : Action, IInternal
    {
        public Purify(int maker, int target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            foreach (var effect in GetTarget().Effects
                         .Where(effect => effect.Category == EffectCategory.Debuff))
                ActionManager.EnqueueAction(new RemoveEffect(effect));
        }
    }
}