using Game.Board.Action.Skills;
using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic.Commanders;
using Game.Interaction;
using Game.UX.UI.ChrysosShop;
using UnityEngine;
using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Internal.Pending
{
    public class ChrysosUpgradeCandidate: Action, IPendingAble, IInternal, ISkills
    {
        private PieceConfig config;

        public readonly PieceType CurrentPiece;
        public readonly PieceRank UpgradeFrom;
        public readonly PieceRank UpgradableTo;
        public readonly byte Cost;
        
        public ChrysosUpgradeCandidate(int caller, int to, int cost) : base(caller, false)
        {
            From = (ushort)caller;
            To = (ushort)to;
            Cost = (byte)cost;

            var cr = gameState.MainBoard[to];
            UpgradableTo = Chrysos.UpgradableTo(cr.pieceRank);
            UpgradeFrom = cr.pieceRank;
            CurrentPiece = cr.type;
        }

        public void CompleteAction()
        {
            var shop = Object.FindAnyObjectByType<ChrysosShop>(FindObjectsInactive.Include);
            if (!shop)
            {
                var canvas = GameObject.Find("IngameUI");
                shop = Object.Instantiate(BoardInteractionUtils.ChrysosShop(), canvas.transform).GetComponent<ChrysosShop>();
            }
            else shop.gameObject.SetActive(true);

            shop.Load((Chrysos)gameState.MainBoard[Caller], this);
        }

        protected override void ModifyGameState()
        {}
    }
}