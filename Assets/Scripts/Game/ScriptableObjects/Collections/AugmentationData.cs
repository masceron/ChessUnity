using Game.Augmentation;
using Game.Common;
using ScrutableObjects;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "AugmentationData", menuName = "ScriptableObjects/Collections/AugmentationData")]
    public class AugmentationData
    {
        [ShowProperties(LockObjectAtRuntime = true)]
        [SerializeField]
        public UDictionary<AugmentationName, AugmentationInfo> augmentationsData;
    }
}
