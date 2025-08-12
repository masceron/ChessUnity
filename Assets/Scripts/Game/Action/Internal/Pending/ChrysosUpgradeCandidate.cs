using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Piece;
using Game.Piece.PieceLogic.Commanders;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.ChrysosShop;

namespace Game.Action.Internal.Pending
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChrysosUpgradeCandidate: Action, IPendingAble, IInternal, ISkills
    {
        private PieceConfig config;

        public readonly PieceType CurrentPiece;
        public readonly PieceRank UpgradeFrom;
        public readonly PieceRank UpgradableTo;
        public readonly byte Cost;
        
        public ChrysosUpgradeCandidate(int maker, int to, int cost) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
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
                var canvas = Object.FindAnyObjectByType<BoardViewer>(FindObjectsInactive.Exclude);
                shop = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.ChrysosShop), canvas.transform).GetComponent<ChrysosShop>();
            }
            else shop.gameObject.SetActive(true);

            shop.Load((Chrysos)BoardUtils.PieceOn(Maker), this);
        }

        protected override void ModifyGameState()
        {}
    }
}