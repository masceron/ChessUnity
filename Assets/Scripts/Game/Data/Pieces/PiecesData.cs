using Game.Common;
using Game.Piece;
using UnityEngine;

namespace Game.Data.Pieces
{
    [CreateAssetMenu(fileName = "PiecesData", menuName = "ScriptableObjects/PiecesData")]
    public class PiecesData: ScriptableObject
    {
        [SerializeField] public UDictionary<PieceType, PieceObject> piecesData;
    }
}