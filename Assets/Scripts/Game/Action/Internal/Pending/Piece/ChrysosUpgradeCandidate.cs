using Cysharp.Threading.Tasks;
using Game.Action.Skills;
using Game.Piece;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
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
            var payload = new ShopPayLoad(GetTargetAsPiece().Type, ((Chrysos)GetMakerAsPiece()).Coin, _cost,
                _upgradableTo, _upgradeFrom);
            
            var chosenUpgrade = await context.OpenMenu<ShopPayLoad, string>(InGameMenuType.ChrysosShop, payload);
            var target = GetTargetAsPiece();

            return chosenUpgrade == null
                ? null
                : new ChrysosUpgrade(GetMakerAsPiece(), GetTargetAsPiece(),
                    new PieceConfig(chosenUpgrade, target.Color, target.Pos), _cost);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            return 0;
        }
    }

    public struct ShopPayLoad
    {
        public readonly string CurrentPiece;
        public readonly int CurrentBalance;
        public readonly int Cost;
        public readonly PieceRank UpgradableTo;
        public readonly PieceRank UpgradeFrom;

        public ShopPayLoad(string currentPiece, int currentBalance, int cost, PieceRank upgradableTo,
            PieceRank upgradeFrom)
        {
            CurrentPiece = currentPiece;
            CurrentBalance = currentBalance;
            Cost = cost;
            UpgradableTo = upgradableTo;
            UpgradeFrom = upgradeFrom;
        }
    }
}