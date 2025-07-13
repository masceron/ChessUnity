using System.Collections.Generic;
using Game.Board.General;
using Game.Board.Piece;
using UnityEngine;

namespace Game.Board.Assets
{
    public class AssetManager : MonoBehaviour
    {
        [SerializeField] private PieceObject[] pieceData;
        public Dictionary<PieceType, PieceObject> PieceData;

        public void Init()
        {
            PieceData = new Dictionary<PieceType, PieceObject>();
            foreach (var piece in pieceData)
            {
                PieceData.Add(piece.type, piece);
            }
        }
    }
}
