using System.Collections.Generic;
using Game.Save.FreePlay;
using MemoryPack;

namespace Game.Save.Player
{
    [MemoryPackable]
    public partial struct Player
    {
        public Dictionary<string, Army.Army> SavedArmies;
        public Dictionary<string, FPPreset> SavedPresets;
        public int Money;
        public List<string> CollectedUnits;
        public List<string> CollectedRelics;
        public List<string> CollectedAugmentations;
    }
}