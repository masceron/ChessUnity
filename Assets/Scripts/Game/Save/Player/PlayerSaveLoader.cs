using System.Collections.Generic;
using System.IO;
using System.Text;
using MemoryPack;
using UnityEngine;

namespace Game.Save.Player
{
    public static class PlayerSaveLoader
    {
        private static readonly string ReadPath = Application.persistentDataPath + "/player.dat";
        private static readonly string WritePath = Application.persistentDataPath + "/temp.dat";
        public static Player Player;

        static PlayerSaveLoader()
        {
            try
            {
                Player = MemoryPackSerializer.Deserialize<Player>(File.ReadAllBytes(ReadPath));
            }
            catch
            {
                MakeNewPlayer();
            }
        }

        public static void Save()
        {
            using (var stream = File.Open(WritePath, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    writer.Write(MemoryPackSerializer.Serialize(Player));
                }
            }
            
            File.Delete(ReadPath);
            File.Move(WritePath, ReadPath);
        }

        private static void MakeNewPlayer()
        {
            Player.SavedArmies = new Dictionary<string, Army.Army>();
        }
    }
}