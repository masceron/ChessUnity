using Game.Common;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Action.Internal
{
    public class Purify : Action, IInternal
    {
        public Purify(Entity maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            foreach (var effect in GetTargetAsPiece().Effects
                         .Where(effect => effect.Category == EffectCategory.Debuff))
                ActionManager.EnqueueAction(new RemoveEffect(effect));
        }
    }
}