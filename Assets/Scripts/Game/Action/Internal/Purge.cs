using System.Linq;
using Game.Common;
using Game.Effects;
using ZLinq;

namespace Game.Action.Internal
{
    public class Purge : Action, IInternal
    {
        public Purge(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }
        protected override void ModifyGameState()
        {
            foreach (var effect in BoardUtils.PieceOn(Target).Effects.Where
            (effect => effect.Category == EffectCategory.Debuff || effect.Category == EffectCategory.Buff))
            {
                ActionManager.EnqueueAction(new RemoveEffect(effect));
            }
        }
    } 
}