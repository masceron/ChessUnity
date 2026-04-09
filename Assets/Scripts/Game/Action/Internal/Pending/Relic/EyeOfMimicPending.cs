using Cysharp.Threading.Tasks;
using Game.Action.Relics;
using Game.Common;
using ZLinq;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EyeOfMimicPending : PendingAction, IRelicAction
    {
        public EyeOfMimicPending(Entity target) : base(null, target)
        {
        }

        protected override async UniTask<Action> BuildAction(ITargetingContext context)
        {
            var firstChosen = GetTargetAsPiece();
            var pieces = BoardUtils.GetAllPieces();
            var ourSide = BoardUtils.OurSide();

            var allowedPosition = firstChosen.Color == ourSide
                ? pieces.Where(p => p.Color != ourSide).Select(p => p.Pos).ToList()
                : pieces.Where(p => p.Color == ourSide).Select(p => p.Pos).ToList();

            context.ClearHighlights();
            context.Highlighter(allowedPosition);
            var secondPiece = await context.NextSelection(allowedPosition.Contains);

            return new EyeOfMimicExecute(firstChosen, BoardUtils.PieceOn(secondPiece));
        }
    }
}