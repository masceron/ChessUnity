using Game.Augmentation;
using UnityEngine;

namespace Game.ScriptableObjects
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [CreateAssetMenu(fileName = "Augmentation", menuName = "ScriptableObjects/Augmentation")]
    public class AugmentationInfo: ScriptableObject
    {
        public AugmentationName Name;
        public AugmentationRarity Rarity;
        public AugmentationSlot Slot;
        public string Key;
        public Texture2D Icon;
    }
}
