using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    public class EpauletteSharkPurify : Effect, IEndTurnTrigger
    {
        private bool yesterdayIsDay;

        public EpauletteSharkPurify(PieceLogic piece) : base(-1, 1, piece, "effect_epaulette_shark_purify")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (!MatchManager.Ins.GameState.IsDay && yesterdayIsDay)
                ActionManager.EnqueueAction(new Purify(Piece.Pos, Piece.Pos));
            yesterdayIsDay = MatchManager.Ins.GameState.IsDay;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}