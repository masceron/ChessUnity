using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class BoardViewer : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset document;
        private PieceLogic selecting;
        private PieceObject selectingPieceObject;

    }
}
