using Game.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Augmentation
{
    [CreateAssetMenu(fileName = "AugmentationSetInfo", menuName = "Game/Augmentation/AugmentationSetInfo")]
    public class AugmentationSetInfo: ScriptableObject
    {
        public AugmentationSetType Type;
        public bool HaveBonus;
        public int RequiredPieces;
        public List<Effect> SetEffects;
    }

    public enum AugmentationSetType
    {
        None,
        One,
        Two,
        Three
    }
}

