using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    public class EpauletteSharkPurify : Effect, IStartTurnTrigger
    {
        private bool _yesterdayIsDay;

        public EpauletteSharkPurify(PieceLogic piece) : base(-1, 1, piece, "effect_epaulette_shark_purify")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAnyTurn;
            _yesterdayIsDay = MatchManager.Ins && BoardUtils.IsDay();
        }

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Buff;

        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            var isDayNow = BoardUtils.IsDay();

            // Day to Night transition
            if (!isDayNow && _yesterdayIsDay)
            {
                ActionManager.EnqueueAction(new Purify(Piece, Piece));
            }

            _yesterdayIsDay = isDayNow;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}