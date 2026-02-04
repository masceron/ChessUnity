using MemoryPack;
using Game.Action.Quiets;

namespace Game.Action
{
    [MemoryPackUnion(0, typeof(NormalMove))]
    public abstract partial class Action
    {
    }
}
