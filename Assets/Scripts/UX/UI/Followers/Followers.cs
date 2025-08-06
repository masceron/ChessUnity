using System;
using System.Collections.Generic;
using Game.Common;
using Game.Data.Pieces;
using Game.Piece;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UX.UI.Followers
{
    public class Followers: Singleton<Followers>, IPointerClickHandler
    {
        [SerializeField] private PiecesData piecesData;
        [SerializeField] private Armies armies;
        [SerializeField] private PieceList list;
        [SerializeField] private PieceInfo pieceInfo;
        [SerializeField] private SRInfo srInfo;
        
        [NonSerialized] private Dictionary<PieceType, PieceObject> PiecesData;

        private bool selecting;
        private PieceType displaying;

        private void Awake()
        {
            PiecesData = new Dictionary<PieceType, PieceObject>(piecesData.piecesData);
            list.Load(PiecesData);
        }

        private void OnDisable()
        {
            pieceInfo.Undisplay();
            selecting = false;
        }

        public void Back(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            UIManager.Ins.Load(CanvasID.PlayMenu);
        }

        public void Select(PieceType type)
        {
            if (selecting)
            {
                selecting = false;
            }
            
            DisplayInfo((int)type);
            selecting = true;
        }

        public void DisplayInfo(int type)
        {
            if (selecting) return;
            
            
            if (type != -1)
            {
                var piece = (PieceType)type;
                if (displaying == piece) return;

                displaying = piece;
                pieceInfo.Display(PiecesData[piece]);
            }
            else
            {
                pieceInfo.Undisplay();   
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            selecting = false;
            DisplayInfo(-1);
        }
    }
}