using Game.Common;
using Game.Piece;
using UnityEngine;

namespace Game.Data.Pieces
{
    [CreateAssetMenu(fileName = "PiecesData", menuName = "ScriptableObjects/PiecesData")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PiecesData: ScriptableObject
    {
        [SerializeField] public UDictionary<PieceType, PieceObject> piecesData;
    }
}