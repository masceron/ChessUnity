using System;
using UnityEngine;

namespace Game.AI.Consider
{
    [Serializable]
    public struct ConsiderationWeight
    {
        public ConsiderationSO Consideration;
        [Range(-100f, 100f)] public float Weight;
    }
}