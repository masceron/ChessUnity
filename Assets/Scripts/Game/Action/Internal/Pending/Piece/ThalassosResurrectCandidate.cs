using Cysharp.Threading.Tasks;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Toolkit.Common;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThalassosResurrectCandidate : PendingAction, ISkills
    {
        public ThalassosResurrectCandidate(PieceLogic maker, int pos) : base(maker, pos)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override async UniTask<Action> BuildAction(ITargetingContext context)
        {
            var chosenType =
                await context.OpenMenu<bool, string>(InGameMenuType.ThalassosShop, GetMakerAsPiece().Color);

            return chosenType == null ? null : new ThalassosResurrect(GetMakerAsPiece(), GetTargetPos(), chosenType);
        }
    }
}