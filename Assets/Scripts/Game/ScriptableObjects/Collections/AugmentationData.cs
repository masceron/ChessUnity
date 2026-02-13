using Game.Augmentation;
using Game.Common;
using ScrutableObjects;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "AugmentationData", menuName = "ScriptableObjects/AugmentationData")]
    public class AugmentationData : ScriptableObject
    {
        [ShowProperties(LockObjectAtRuntime = true)] [SerializeField]
        public UDictionary<AugmentationName, AugmentationInfo> augmentationsData;
    }
}