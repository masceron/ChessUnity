using Game.Board.General;
using UnityEngine;

namespace Game.Board.Piece
{
    [CreateAssetMenu(fileName = "PieceObject", menuName = "ScriptableObjects/PieceObject")]
    public class PieceObject : ScriptableObject
    {
        [SerializeField] public PieceType type;
        [SerializeField] public GameObject prefab;
        [SerializeField] public Vector3 defaultTransform;

        [SerializeField] public PieceRank rank;
        [SerializeField] public sbyte moveRange;
        [SerializeField] public sbyte attackRange;
    }
}