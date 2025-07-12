using Game.Board.General;
using UnityEngine;

namespace Resources.ScriptableObjects.Pieces
{
    [CreateAssetMenu(fileName = "PieceObject", menuName = "ScriptableObjects/PieceObject")]
    public class PieceObject : ScriptableObject
    {
        [SerializeField] public GameObject prefab;
        [SerializeField] public Vector3 defaultTransform;
    }
}