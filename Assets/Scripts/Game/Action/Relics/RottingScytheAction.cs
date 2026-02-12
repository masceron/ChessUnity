using MemoryPack;
using Game.Action.Internal;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class RottingScytheAction : Action, IRelicAction
    {
        public RottingScytheAction(int maker) : base(maker)
        {
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(Maker));
        }
    }
}