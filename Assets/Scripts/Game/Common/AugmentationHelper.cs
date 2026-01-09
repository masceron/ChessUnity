using System;
using Game.Augmentation;

namespace Game.Common
{
    public static class AugmentationHelper
    {
        public static bool TryStringToAugmentationName(string name, out AugmentationName augmentationName)
        {
            return Enum.TryParse(name, out augmentationName);
        }

        public static bool TryAugmentationNameToString(AugmentationName augmentationName, out string name)
        {
            name = augmentationName.ToString();
            return true;
        }
    }
}
