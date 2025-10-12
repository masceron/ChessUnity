using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Debuffs
{
    public class Infected : Effect, IEndTurnEffect
    {
        private int turnCounter = 0;
        public Infected(PieceLogic piece) : base(-1, 1, piece, EffectName.Infected)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            turnCounter++;

            if (turnCounter == 3)
            {
                // InfectedActivate();
            }
        }

        private void InfectedActivate()
        {
            var (rank, file) = RankFileOf(Piece.Pos);
            ActionManager.ExecuteImmediately(new KillPiece(Piece.Pos));
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null || pOn == Piece) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Infected(pOn)));
            }
        }
    }
}