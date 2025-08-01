using System.Linq;
using DG.Tweening;
using Game.Board.Action.Internal.Pending;
using Game.Board.Action.Skills;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using Game.Board.Piece.PieceLogic.Commanders;
using Game.Interaction;
using TMPro;
using UnityEngine;
using static Game.Board.General.MatchManager;

namespace Game.UX.UI.ChrysosShop
{
    public class ChrysosShop: MonoBehaviour
    {
        [SerializeField] private GameObject shopItem;
        [SerializeField] private TMP_Text balanceField;
        [SerializeField] private TMP_Text costField;
        [SerializeField] private GameObject itemLine;
        private PieceLogic chrysos;
        private ChrysosUpgradeCandidate candidate;
        private byte cost;

        private void OnEnable()
        {
            var rect = (RectTransform)transform.GetChild(0);
            rect.anchoredPosition = new Vector2(-50, 0);
            rect.DOAnchorPos(Vector3.zero, 0.3f);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            ((RectTransform)transform.GetChild(0)).anchoredPosition = new Vector2(-50, 0);
        }

        public void Load(Chrysos c, ChrysosUpgradeCandidate cd)
        {
            balanceField.text = "Balance: " + c.Coin;
            costField.text = "Cost: " + cd.Cost;
            chrysos = c;
            candidate = cd;
            cost = cd.Cost;
            
            var upgradableTo = (from piece in assetManager.PieceData.Values where piece.rank == candidate.UpgradableTo select piece.type).ToList();
            if (cd.UpgradeFrom == PieceRank.Champion) upgradableTo.Remove(cd.CurrentPiece);
            var already = itemLine.transform.childCount;
            var needed = upgradableTo.Count;
            
            if (already < needed)
            {
                for (var i = 1; i <= needed - already; i++)
                {
                    Instantiate(shopItem, itemLine.transform, true);
                }
            }
            else if (already > needed)
            {
                var index = already - 1;
                for (var i = 1; i <= already - needed; i++)
                {
                    Destroy(itemLine.transform.GetChild(index).gameObject);
                    index--;
                }
            }

            for (var i = 0; i < needed; i++)
            {
                itemLine.transform.GetChild(i).GetComponent<ChrysosShopItem>().Load(upgradableTo[i]);
            }
        }

        public void Buy(PieceType type)
        {
            BoardInteractionUtils.ExecuteAction(new ChrysosUpgrade(chrysos.Pos, new PieceConfig(type, chrysos.Color, candidate.To), cost));
            Disable();
        }
    }
}