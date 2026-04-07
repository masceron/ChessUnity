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
            var firstPiece = GetTargetAsPiece();
            var firstIsAlly = firstPiece.Color == GetMakerAsPiece().Color;
            
            var secondPos = await context.NextSelection(pos => 
            {
                var p = BoardUtils.PieceOn(pos);
                if (p == null) return false;
            
                var isAlly = p.Color == GetMakerAsPiece().Color;
                return firstIsAlly ? !isAlly : isAlly;
            });
            
            return new TemperantiaSwap(GetMakerAsPiece(), firstPiece.Pos, secondPos);
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
    }
}