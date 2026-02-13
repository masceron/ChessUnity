using System.Collections.Generic;
using ScrutableObjects;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "RelicsData", menuName = "ScriptableObjects/RelicsData")]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RelicsData : ScriptableObject
    {
        [ShowProperties(LockObjectAtRuntime = true)] [SerializeField]
        public List<RelicInfo> relicsData;
    }
}