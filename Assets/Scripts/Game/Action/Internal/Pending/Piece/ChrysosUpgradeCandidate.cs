using Cysharp.Threading.Tasks;
using Game.Action.Skills;
using Game.Piece;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Toolkit.Common;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChrysosUpgradeCandidate : PendingAction, ISkills
    {
        private readonly int _cost;
        private readonly PieceRank _upgradableTo;
        private readonly PieceRank _upgradeFrom;

        public ChrysosUpgradeCandidate(PieceLogic maker, PieceLogic to) : base(maker, to)
        {
            var toUpgrade = GetTargetAsPiece();
            (_upgradableTo, _cost) = Chrysos.UpgradableTo(toUpgrade.PieceRank);
            _upgradeFrom = toUpgrade.PieceRank;
        }

        protected override async UniTask<Action> BuildAction(ITargetingContext context)
        {
            var payload = new ShopPayLoad(_upgradableTo, _upgradeFrom);

            var chosenUpgrade = await context.OpenMenu<PieceConfig?, ShopPayLoad>(InGameMenuType.ChrysosShop, payload);

            return chosenUpgrade == null
                ? null
                : new ChrysosUpgrade(GetMakerAsPiece(), GetTargetAsPiece(), chosenUpgrade.Value, _cost);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            return 0;
        }
    }

    public struct ShopPayLoad
    {
        public readonly PieceRank UpgradableTo;
        public readonly PieceRank UpgradeFrom;

        public ShopPayLoad(PieceRank upgradableTo, PieceRank upgradeFrom)
        {
            UpgradableTo = upgradableTo;
            UpgradeFrom = upgradeFrom;
        }
    }
}