using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Others
{
    public class MelibePassive : Effect, IAfterPieceActionTrigger
    {
        public MelibePassive(PieceLogic piece) : base(-1, 1, piece, "effect_melibe_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetMakerAsPiece() != Piece || action.Result != ResultFlag.Success) return;
            var targetPos = action.GetTargetPos();
            var targetRank = RankOf(targetPos);
            var targetFile = FileOf(targetPos);

            var leftTargetFile = targetFile - 1;
            var pieceOnLeft = PieceOn(IndexOf(targetRank, leftTargetFile));
            if (pieceOnLeft != null && pieceOnLeft != Piece)
                ActionManager.EnqueueAction(new ApplyEffect(new Silenced(pieceOnLeft), Piece));

            var rightTargetFile = targetFile + 1;
            var pieceOnRight = PieceOn(IndexOf(targetRank, rightTargetFile));
            if (pieceOnRight != null && pieceOnRight != Piece)
                ActionManager.EnqueueAction(new ApplyEffect(new Silenced(pieceOnRight), Piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}