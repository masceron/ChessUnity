using System.Linq;
using Game.Common;
using Game.Effects;

namespace Game.Action.Internal
{
    public class Purify: Action, IInternal
    {
        public Purify(int maker, int target) : base(maker)
        {
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            var piece = BoardUtils.PieceOn(Target);
            piece.CanUseSkill = true;
            foreach (var effect in piece.Effects.Where(effect => effect.Category == EffectCategory.Debuff))
            {
                ActionManager.EnqueueAction(new RemoveEffect(effect));
            }
        }
    }
}