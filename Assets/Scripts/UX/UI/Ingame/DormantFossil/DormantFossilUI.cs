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

        private readonly List<PieceType> spawnPiece = new()
        {
            PieceType.Helicoprion,
            PieceType.Anomalocaris,
            PieceType.Archelon 
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

        public void Choose(PieceType type)
        {
            Debug.Log("choosed");
            var color = BoardUtils.ColorOfPiece(piecePos);
            
            ActionManager.EnqueueAction(new KillPiece(piecePos));
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(type, color, piecePos)));
            
            Disable();
        }
    }
}