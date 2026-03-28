using System.Collections.Generic;
using Game.Action.Internal.Pending;
using Game.Action.Skills;
using Game.Common;
using Game.Piece;
using PrimeTween;
using UnityEngine;

namespace UX.UI.Ingame.DormantFossil
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DormantFossilUI : IngamePendingMenu
    {
        [SerializeField] private GameObject chooseField;
        [SerializeField] private GameObject pieceItem;

        private readonly List<string> spawnPiece = new()
        {
            "piece_helicoprion",
            "piece_anomalocaris",
            "piece_archelon"
        };

        private int piecePos;

        protected override PendingAction PendingAction { get; set; }

        private void OnEnable()
        {
            var rect = (RectTransform)transform.GetChild(0);
            rect.anchoredPosition = new Vector2(-50, 0);
            Tween.UIAnchoredPosition(rect, Vector3.zero, 0.3f);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            ((RectTransform)transform.GetChild(0)).anchoredPosition = new Vector2(-50, 0);
        }

        public void Load(int spawnPos)
        {
            piecePos = spawnPos;

            for (var i = 0; i < 3; ++i)
            {
                if (chooseField.transform.childCount >= 3) continue;
                Instantiate(pieceItem, chooseField.transform, true);
                chooseField.transform.GetChild(i).GetComponent<DormantFossilItem>().Load(spawnPiece[i]);
            }
        }

        public void Choose(string type)
        {
            var color = BoardUtils.PieceOn(piecePos).Color;

            PendingAction.CommitResult(new DormantFossilAwake(piecePos, new PieceConfig(type, color, piecePos)));

            Disable();
        }
    }
}