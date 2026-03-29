using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Action.Internal
{
    public class Purify : Action, IInternal
    {
        public Purify(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            foreach (var effect in ((PieceLogic)GetTarget()).Effects
                         .Where(effect => effect.Category == EffectCategory.Debuff))
                ActionManager.EnqueueAction(new RemoveEffect(effect));
        }
    }
}