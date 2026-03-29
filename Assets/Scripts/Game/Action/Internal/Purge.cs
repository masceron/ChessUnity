using Game.Common;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Action.Internal
{
    public class Purge : Action, IInternal
    {
        public Purge(PieceLogic maker, PieceLogic to) : base(maker, to)
        {
        }

        protected override void ModifyGameState()
        {
            foreach (var effect in ((PieceLogic)GetTarget()).Effects.Where
                         (effect => effect.Category is EffectCategory.Debuff or EffectCategory.Buff))
                ActionManager.EnqueueAction(new RemoveEffect(effect));
        }
    }
}