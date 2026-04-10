using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "RelicsData", menuName = "ScriptableObjects/RelicsData")]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RelicsData : ScriptableObject
    {
        [SerializeField]
        public List<RelicInfo> relicsData;
    }
}