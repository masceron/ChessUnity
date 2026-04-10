using Game.Augmentation;
using Game.Common;
using UnityEngine;

namespace Game.ScriptableObjects.Collections
{
    [CreateAssetMenu(fileName = "AugmentationData", menuName = "ScriptableObjects/AugmentationData")]
    public class AugmentationData : ScriptableObject
    {
        [SerializeField]
        public UDictionary<AugmentationName, AugmentationInfo> augmentationsData;
    }
}