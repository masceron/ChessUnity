using Game.Action;
using Game.Action.Internal;
using Game.Augmentation;
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
        
        public override void OnCallPieceAction(Action.Action action)
        {
            turnCounter++;

            if (turnCounter % 3 == 0)
            {
                var pieceTarget = PieceOn(action.Target);
                if (pieceTarget != null && pieceTarget.HasAugmentation(AugmentationName.HemolymphFilter))
                {
                    return;
                }
                InfectedActivate();
            }
        }

        private void InfectedActivate()
        {
            var (rank, file) = RankFileOf(Piece.Pos);
            ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null || pOn == Piece) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Infected(pOn)));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 200;
        }
    
    }
}