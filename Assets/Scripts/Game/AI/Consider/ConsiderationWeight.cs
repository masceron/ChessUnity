using System;
using UnityEngine;

namespace Game.AI.Consider
{
    [Serializable]
    public struct ConsiderationWeight
    {
        public ConsiderationSO Consideration;
        [Range(0f, 10f)] public float Weight;
    }
}