using System.Collections.Generic;
using Game.Save.Player;

namespace Game.Save.FreePlay
{
    public static class FreePlaySaveLoader
    {
        public static void Save(FPPreset army)
        {
            PlayerSaveLoader.Player.SavedPresets[army.Name] = army;
            PlayerSaveLoader.Save();
        }

        public static Dictionary<string, FPPreset> ReadAll()
        {
            return PlayerSaveLoader.Player.SavedPresets;
        }

        public static FPPreset Read(string name)
        {
            return PlayerSaveLoader.Player.SavedPresets[name];
        }

        public static bool Exists(string name)
        {
            return PlayerSaveLoader.Player.SavedPresets.ContainsKey(name);
        }

        public static void Remove(string name)
        {
            PlayerSaveLoader.Player.SavedPresets.Remove(name);
            PlayerSaveLoader.Save();
        }
    }
}