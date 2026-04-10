using System.Collections.Generic;
using Game.Action.Internal.Pending;
using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using PrimeTween;
using Unity.Properties;
using UnityEngine;

namespace UX.UI.Ingame.CoralTome
{
    public class CoralTomeUI : IngamePendingMenu
    {
        [SerializeField] private GameObject chooseField;
        [SerializeField] private GameObject pieceItem;

        private readonly List<string> _spawnPiece = new()
        {
            "piece_barnacle",
            "piece_melibe",
            "piece_blue_dragon"
        };

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

        public void Load()
        {
            for (var i = 0; i < 3; ++i)
            {
                if (chooseField.transform.childCount >= 3) continue;
                Instantiate(pieceItem, chooseField.transform, true);
                chooseField.transform.GetChild(i).GetComponent<CoralTomeItem>().Load(_spawnPiece[i]);
            }
        }

        public void Choose(string type)
        {
            Disable();
            var color = BoardUtils.OurSide();
            foreach (var pos in BoardUtils.AllSidePos(color))
            {
                TileManager.Ins.MarkAsMoveable(pos);
                var relic = BoardUtils.GetRelicOf(color);
                if (relic is not Game.Relics.CoralTome coralTome) continue;

                BoardViewer.ListOf.Add(new CoralTomeAction(color, type, pos));
            }

            BoardViewer.Selecting = -2;
            BoardViewer.SelectingFunction = 4;
        }
    }
}