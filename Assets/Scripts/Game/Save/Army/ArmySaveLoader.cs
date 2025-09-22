using System.Collections.Generic;
using Game.Save.Player;

namespace Game.Save.Army
{
    public static class ArmySaveLoader
    {
        public static void Save(Army army)
        {
            PlayerSaveLoader.Player.SavedArmies[army.Name] = army;
            PlayerSaveLoader.Save();
        }

        public static Dictionary<string, Army> ReadAll()
        {
            return PlayerSaveLoader.Player.SavedArmies;
        }

        public static Army Read(string name)
        {
            return PlayerSaveLoader.Player.SavedArmies[name];
        }

        public static bool Exists(string name)
        {
            return PlayerSaveLoader.Player.SavedArmies.ContainsKey(name);
        }

        public static void Remove(string name)
        {
            PlayerSaveLoader.Player.SavedArmies.Remove(name);
            PlayerSaveLoader.Save();
        }
    }
}