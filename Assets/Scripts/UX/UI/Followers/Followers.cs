using System;
using System.Collections.Generic;
using Game.Common;
using Game.Data.Pieces;
using Game.Piece;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UX.UI.Followers
{
    public class Followers: Singleton<Followers>
    {
        [SerializeField] private PiecesData piecesData;
        [SerializeField] private Armies armies;
        [SerializeField] private PieceList list;
        [SerializeField] private PieceInfo info;
        
        [NonSerialized] private Dictionary<PieceType, PieceObject> PiecesData;

        private void Awake()
        {
            PiecesData = new Dictionary<PieceType, PieceObject>(piecesData.piecesData);
            list.Load(PiecesData);
        }

        public void Back(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            UIManager.Ins.Load(CanvasID.PlayMenu);
        }
    }
}