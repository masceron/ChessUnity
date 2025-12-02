using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Effects.Others
{
    public class MelibePassive : Effect
    {
        public MelibePassive(PieceLogic piece) : base(-1, 1, piece, "effect_melibe_passive")
        {
            
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            var targetPos = action.Target;
            var targetRank = RankOf(targetPos);
            var targetFile = FileOf(targetPos);
            
            var leftTargetFile = targetFile - 1;
            var pieceOnLeft = PieceOn(IndexOf(targetRank, leftTargetFile));
            if (pieceOnLeft != null && pieceOnLeft != Piece)
            {
                ActionManager.ExecuteImmediately(new ApplyEffect(new Silenced(pieceOnLeft)));
            }
            
            var rightTargetFile = targetFile + 1;
            var pieceOnRight = PieceOn(IndexOf(targetRank, rightTargetFile));
            if (pieceOnRight != null && pieceOnRight != Piece)
            {
                ActionManager.ExecuteImmediately(new ApplyEffect(new Silenced(pieceOnRight)));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}