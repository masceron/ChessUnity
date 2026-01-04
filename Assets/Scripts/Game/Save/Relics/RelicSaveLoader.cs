using System.Collections.Generic;
using Game.Save.Player;

namespace Game.Save.Relics
{
    public static class RelicSaveLoader
    {
        public static List<string> GetCollectedRelics()
        {
            return PlayerSaveLoader.Player.CollectedRelics;
        }

        public static void AddRelic(string name)
        {
            if (!PlayerSaveLoader.Player.CollectedRelics.Contains(name))
            {
                PlayerSaveLoader.Player.CollectedRelics.Add(name);
                PlayerSaveLoader.Save();
            }
        }

        public static void RemoveRelic(string name)
        {
            if (PlayerSaveLoader.Player.CollectedRelics.Remove(name))
            {
                PlayerSaveLoader.Save();
            }
        }

        public static bool HasRelic(string name)
        {
            return PlayerSaveLoader.Player.CollectedRelics.Contains(name);
        }
    }
}
