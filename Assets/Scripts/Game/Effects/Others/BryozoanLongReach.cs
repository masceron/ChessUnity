using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Others
{
    public class BryozoanLongReach : Effect, IStartTurnTrigger
    {
        private const int BuffRange = 2;
        private const int BuffDuration = 3;
        private const int BuffStrength = 1;

        public BryozoanLongReach(PieceLogic piece)
            : base(-1, -1, piece, "effect_bryozoan_long_reach")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
        }

        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Buff;
        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            var (rank, file) = RankFileOf(Piece.Pos);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, BuffRange))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);

                if (pOn == null || pOn == Piece || pOn.Color != Piece.Color) 
                    continue;

                ActionManager.EnqueueAction(
                    new ApplyEffect(new LongReach(pOn, BuffDuration, BuffStrength), Piece)
                );
            }
        }
    }
}