using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Action.Internal
{
    public class Nullify : Action, IInternal
    {
        public Nullify(PieceLogic maker, PieceLogic to) : base(maker, to)
        {
        }

        protected override void ModifyGameState()
        {
            foreach (var effect in GetTargetAsPiece().Effects
                         .Where(effect => effect.Category == EffectCategory.Buff))
                ActionManager.EnqueueAction(new RemoveEffect(effect));
        }
    }
}