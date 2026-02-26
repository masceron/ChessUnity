using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Debuffs
{
    public class Infected : Effect, IEndTurnTrigger
    {
        private int radius = 1;
        private int turnCounter;
        private int turnToActivate = 3;

        public Infected(PieceLogic piece) : base(-1, 1, piece, "effect_infected")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Debuff;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            turnCounter++;

            if (turnCounter % turnToActivate == 0) InfectedActivate();
        }

        private void InfectedActivate()
        {
            var (rank, file) = RankFileOf(Piece.Pos);
            ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, radius))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null || pOn == Piece) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Infected(pOn), Piece));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 200;
        }

        public void SetTurnCounter(int turnValue)
        {
            if (turnValue < 1) return;
            turnToActivate = turnValue;
        }

        public void SetRadius(int radiusValue)
        {
            if (radiusValue < 1) return;
            radius = radiusValue;
        }
    }
}