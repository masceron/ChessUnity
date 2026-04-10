using MemoryPack;

namespace Game.Save.Player
{
    [MemoryPackable]
    public partial struct Settings
    {
        public int TimeToLockTooltip;
    }
}