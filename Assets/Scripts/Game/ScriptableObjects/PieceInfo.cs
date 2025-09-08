using Game.Piece;
using UnityEngine;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Piece", menuName = "ScriptableObjects/Piece")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceInfo : ScriptableObject
    {
        [SerializeField] public PieceType type;
        [SerializeField] public GameObject prefab;

        [SerializeField] public PieceRank rank;
        [SerializeField] public byte moveRange;
        [SerializeField] public byte attackRange;
        
        [SerializeField] public Texture2D movePattern;
        [SerializeField] public Texture2D capturePattern;
        
        [SerializeField] public string key;
        [SerializeField] public bool hasSkill;
        [SerializeField] public sbyte normalSkillCooldown;
    }
}