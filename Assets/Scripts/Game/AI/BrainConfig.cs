using System;
using UnityEngine;

namespace Game.AI
{
    [Serializable]
    public struct ConsiderationWeight
    {
        public ConsiderationSO Consideration;
        [Range(0f, 10f)] public float Weight;
    }

    [CreateAssetMenu(menuName = "AI/BrainConfig")]
    public class BrainConfig : ScriptableObject
    {
        public ConsiderationWeight[] Considerations = Array.Empty<ConsiderationWeight>();
    }
}
