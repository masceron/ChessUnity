using System;
using System.Collections.Generic;
using System.Linq;
using Game.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Common
{
    public static class AugmentationHelper
    {
        private static readonly Dictionary<AugmentationName, Type> AugmentationTypeCache;

        static AugmentationHelper()
        {
            AugmentationTypeCache = new Dictionary<AugmentationName, Type>();

            var augmentationTypes = typeof(Augmentation.Augmentation).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Augmentation.Augmentation)))
                .ToArray();

            foreach (AugmentationName name in Enum.GetValues(typeof(AugmentationName)))
            {
                if (name == AugmentationName.None) continue;

                var type = augmentationTypes.FirstOrDefault(t => string.Equals(t.Name, name.ToString(), StringComparison.OrdinalIgnoreCase));
                if (type != null)
                {
                    AugmentationTypeCache[name] = type;
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"[AugmentationHelper] Found Enum 'AugmentationName.{name}' but could not find a matching class named '{name}' that inherits from Augmentation.");
                }
            }
        }

        public static bool TryStringToAugmentationName(string name, out AugmentationName augmentationName)
        {
            return Enum.TryParse(name, out augmentationName);
        }

        public static bool TryAugmentationNameToString(AugmentationName augmentationName, out string name)
        {
            name = augmentationName.ToString();
            return true;
        }

        public static Augmentation.Augmentation NameToNewAugmentation(AugmentationName augmentationName)
        {
            if (AugmentationTypeCache.TryGetValue(augmentationName, out var type))
            {
                return (Augmentation.Augmentation)Activator.CreateInstance(type);
            }
            return null;
        }

        public static Augmentation.Augmentation NameToAugmentationInPiece(AugmentationName augmentationName, PieceLogic piece)
        {
            return piece.GetAugmentation(augmentationName);
        }
    }
}
