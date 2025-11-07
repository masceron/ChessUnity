using System.Collections.Generic;
using MemoryPack;
using Game.Save.Army;

namespace Game.Save.Player
{
    [MemoryPackable]
    public partial struct Player
    {
        public Dictionary<string, Army.Army> SavedArmies;
        
    }
}