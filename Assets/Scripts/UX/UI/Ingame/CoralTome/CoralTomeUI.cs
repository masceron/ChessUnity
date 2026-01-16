using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Managers;
using Game.Piece;
using PrimeTween;
using UnityEngine;
using UX.UI.Ingame.DormantFossil;
using Color = UnityEngine.Color;

namespace UX.UI.Ingame.CoralTome
{
    public class CoralTomeUI : MonoBehaviour
    {
        [SerializeField] private GameObject chooseField;
        [SerializeField] private GameObject pieceItem;

        private readonly List<string> spawnPiece = new()
        {
            "piece_barnacle",
            "piece_melibe",
            "piece_blue_dragon" 
        };
        
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

        public void Load()
        {
            for (var i = 0; i < 3; ++i)
            {
                if (chooseField.transform.childCount < 3)
                {
                    Instantiate(pieceItem, chooseField.transform, true);
                    chooseField.transform.GetChild(i).GetComponent<CoralTomeItem>().Load(spawnPiece[i]);
                }
            }

        }

        public void Choose(string type)
        {
            Disable();
            //TODO: fix options to choose (all the active tiles of our side instead of all the pieces on the board)
            foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
            {
                if (piece == null) continue;
                TileManager.Ins.MarkAsMoveable(piece.Pos);
                var relic = BoardUtils.GetRelicOf(true);
                //TODO: fix color of piece which uses the relic (not always be true).
                if (relic is Game.Action.Internal.Pending.Relic.CoralTome coralTome)
                {
                    var pending = new CoralTomePending(coralTome, piece.Pos, type);
                    BoardViewer.ListOf.Add(pending);
                }
            }
            BoardViewer.Selecting = -2;
            BoardViewer.SelectingFunction = 4;
        }
    }
}