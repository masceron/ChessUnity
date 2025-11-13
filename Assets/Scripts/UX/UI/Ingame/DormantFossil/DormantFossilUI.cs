using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece;
using PrimeTween;
using UnityEngine;

namespace UX.UI.Ingame.DormantFossil
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    
    public class DormantFossilUI : MonoBehaviour
    {
        private ushort piecePos;
        [SerializeField] private GameObject chooseField;
        [SerializeField] private GameObject pieceItem;

        private readonly List<string> spawnPiece = new()
        {
            "piece_helicoprion",
            "piece_anomalocaris",
            "piece_archelon" 
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

        public void Load(ushort spawnPos)
        {
            piecePos = spawnPos;
            
            for (var i = 0; i < 3; ++i)
            {
                Instantiate(pieceItem, chooseField.transform, true);
                chooseField.transform.GetChild(i).GetComponent<DormantFossilItem>().Load(spawnPiece[i]);
            }

        }

        public void Choose(string type)
        {
            var color = BoardUtils.ColorOfPiece(piecePos);
            
            ActionManager.EnqueueAction(new KillPiece(piecePos));
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(type, color, piecePos)));
            
            Disable();
        }
    }
}