using Game.Board.Action.Skills;
using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic.Commanders;
using Game.Common;
using Game.UX.UI.ChrysosShop;
using UnityEngine;

namespace Game.Board.Action.Internal.Pending
{
    public class ChrysosUpgradeCandidate: Action, IPendingAble, IInternal, ISkills
    {
        private PieceConfig config;

        public readonly PieceType CurrentPiece;
        public readonly PieceRank UpgradeFrom;
        public readonly PieceRank UpgradableTo;
        public readonly byte Cost;
        
        public ChrysosUpgradeCandidate(int from, int to, int cost) : base(from, false)
        {
            From = (ushort)from;
            To = (ushort)to;
            Cost = (byte)cost;

            var cr = BoardUtils.PieceOn(to);
            UpgradableTo = Chrysos.UpgradableTo(cr.PieceRank);
            UpgradeFrom = cr.PieceRank;
            CurrentPiece = cr.Type;
        }

        public void CompleteAction()
        {
            var shop = Object.FindAnyObjectByType<ChrysosShop>(FindObjectsInactive.Include);
            if (!shop)
            {
                var canvas = GameObject.Find("IngameUI");
                shop = Object.Instantiate(MatchManager.Ins.InputProcessor.chrysosShopUI, canvas.transform).GetComponent<ChrysosShop>();
            }
            else shop.gameObject.SetActive(true);

            shop.Load((Chrysos)BoardUtils.PieceOn(From), this);
        }

        protected override void ModifyGameState()
        {}
    }
}