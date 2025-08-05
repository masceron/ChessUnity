using Game.Piece;
using UnityEngine;

namespace Game.Data.Pieces
{
    [CreateAssetMenu(fileName = "PieceObject", menuName = "ScriptableObjects/PieceObject")]
    public class PieceObject : ScriptableObject
    {
        [SerializeField] public string pieceName;
        [SerializeField] public PieceType type;
        [SerializeField] public GameObject prefab;

        [SerializeField] public PieceRank rank;
        [SerializeField] public sbyte moveRange;
        [SerializeField] public sbyte attackRange;

        [SerializeField] public Texture2D movePattern;
        [SerializeField] public Texture2D capturePattern;

        [SerializeField] public string skillName;
        [SerializeField, TextArea] public string skillDescription;
        [SerializeField] public sbyte normalSkillCooldown;
    }
}