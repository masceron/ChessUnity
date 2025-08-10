using System.Collections.Generic;
using System.IO;
using System.Text;
using MemoryPack;
using UnityEngine;

namespace Game.Save.Army
{
    public static class ArmySaveLoader
    {
        private static readonly string Path = Application.persistentDataPath + "/Armies.bin";
        private static readonly Dictionary<string, Army> Dict;
        
        static ArmySaveLoader()
        {
            try
            {
                Dict = MemoryPackSerializer.Deserialize<Dictionary<string, Army>>(File.ReadAllBytes(Path));
            }
            catch
            {
                Dict = new Dictionary<string, Army>();
            }
        }
        
        public static void Save(Army army)
        {
            using var stream = File.Open(Path, FileMode.Create);
            using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
            Dict[army.Name] = army;
            
            writer.Write(MemoryPackSerializer.Serialize(Dict));
        }

        public static Dictionary<string, Army> ReadAll()
        {
            return Dict;
        }

        public static Army Read(string name)
        {
            return Dict[name];
        }

        public static void Remove(string name)
        {
            using var stream = File.Open(Path, FileMode.Create);
            using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
            Dict.Remove(name);
            
            writer.Write(MemoryPackSerializer.Serialize(Dict));
        }
    }
}