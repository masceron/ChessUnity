using Game.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.AI.Consider;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Piece", menuName = "ScriptableObjects/Piece")]
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PieceInfo : ScriptableObject
    {
        [SerializeField] public string key;
        [SerializeField] public GameObject prefab;
        [SerializeField] public PieceRank rank;
        [SerializeField] public byte moveRange;
        [SerializeField] public byte attackRange;
        [SerializeField] public Texture2D movePattern;
        [SerializeField] public Texture2D capturePattern;
        
        [SerializeField] public bool hasSkill;
        [SerializeField] public sbyte normalSkillCooldown;
        public UDictionary<SkillStat, int> otherSkillStats;
        [SerializeField] public AugmentationSlotMask availableSlots;
        [SerializeField] public string logicClassName;
        [SerializeField] public int baseValue;
        [SerializeField] public List<ConsiderationWeight> considerations;
        [Flags]
        public enum AugmentationSlotMask 
        { 
            None = 0, 
            Optic = 1 << 0, 
            Neural = 1 << 1, 
            Blood = 1 << 2, 
            Fin = 1 << 3, 
            Chassis = 1 << 4 
        }
    }
}