using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumboldtSquidPassive : Effect, IAfterPieceActionTrigger
    {
        private int _count;

        public HumboldtSquidPassive(PieceLogic piece) : base(-1, 1, piece, "effect_humboldt_squid_passive")
        {
            _count = 0;
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.Maker == Piece.Pos
                && action is ICaptures
                && (action.Result == ResultFlag.Blocked
                    || action.Result == ResultFlag.HardenedBlock
                    || action.Result == ResultFlag.Parry))
            {
                var target = PieceOn(action.Target);
                if (target != null && target.Color != Piece.Color)
                    ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(5, target), Piece));
            }

            foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
            {
                if (piece == null) continue;
                if (piece.Effects.Any(e => e.EffectName == "effect_bleeding")) _count++;
            }

            if (_count >= 5)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Frenzied(Piece), Piece));
                _count = 0;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}