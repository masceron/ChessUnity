using System.Collections.Generic;
using Game.Save.Player;

namespace Game.Save.Augmentation
{
    public static class AugmentationSaveLoader
    {
        public static List<string> GetCollectedAugmentations()
        {
            return PlayerSaveLoader.Player.CollectedAugmentations;
        }

        public static void AddAugmentation(string name)
        {
            if (!PlayerSaveLoader.Player.CollectedAugmentations.Contains(name))
            {
                PlayerSaveLoader.Player.CollectedAugmentations.Add(name);
                PlayerSaveLoader.Save();
            }
        }

        public static void RemoveAugmentation(string name)
        {
            if (PlayerSaveLoader.Player.CollectedAugmentations.Remove(name)) PlayerSaveLoader.Save();
        }

        public static bool HasAugmentation(string name)
        {
            return PlayerSaveLoader.Player.CollectedAugmentations.Contains(name);
        }
    }
}