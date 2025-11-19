using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Debuffs
{
    public class Infected : Effect
    {
        private int turnCounter;
        
        public Infected(PieceLogic piece) : base(-1, 1, piece, "effect_infected")
        {
            
        }
        
        public override void OnCallPieceAction(Action.Action lastMainAction)
        {
            turnCounter++;

            if (turnCounter == 3)
            {
                InfectedActivate();
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
                ActionManager.ExecuteImmediately(new ApplyEffect(new Infected(pOn)));
            }
        }
    }
}