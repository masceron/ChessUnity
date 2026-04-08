using Cysharp.Threading.Tasks;
using Game.Action.Skills;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemperantiaPending : PendingAction, ISkills
    {
        public TemperantiaPending(PieceLogic maker, PieceLogic firstTarget) : base(maker, firstTarget)
        {
        }

        protected override async UniTask<Action> BuildAction(ITargetingContext context)
        {
            var maker = GetMakerAsPiece();
            var firstPiece = GetTargetAsPiece();
            var firstIsAlly = firstPiece.Color == GetMakerAsPiece().Color;
            foreach (var piece in BoardUtils.GetAllPieces())
            {
                if (firstIsAlly)
                {
                    if (piece.Color == maker.Color) context.ClearHighlight(piece.Pos);
                }
                else if (piece.Color != maker.Color) context.ClearHighlight(piece.Pos);
            }

            var secondPos = await context.NextSelection(pos =>
            {
                var p = BoardUtils.PieceOn(pos);
                if (p == null) return false;

                var isAlly = p.Color == GetMakerAsPiece().Color;
                return firstIsAlly ? !isAlly : isAlly && pos != GetFrom();
            });

            return new TemperantiaSwap(GetMakerAsPiece(), firstPiece, BoardUtils.PieceOn(secondPos));
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
    }
}