using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SlimeheadPassive : Effect, IAfterPieceActionTrigger
    {
        public SlimeheadPassive(PieceLogic piece) : base(-1, 1, piece, "slimehead_passive")
        {
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 50;
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Target != Piece.Pos || action.Result != ResultFlag.Success) return;
            var maker = BoardUtils.PieceOn(action.Maker);
            var buffEffect = maker.Effects.Count(t => t.Category == EffectCategory.Buff);

            if (buffEffect < 2) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Infected(maker), Piece));
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, maker), Piece));
        }
    }
}