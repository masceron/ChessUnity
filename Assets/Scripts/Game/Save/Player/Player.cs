using System.Collections.Generic;
using MemoryPack;

namespace Game.Save.Player
{
    [MemoryPackable]
    public partial struct Player
    {
        public Dictionary<string, Army.Army> SavedArmies;
    }
}