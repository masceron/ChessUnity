using MemoryPack;
using Game.Action.Internal;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class RottingScytheAction : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private RottingScytheAction() { }

        public RottingScytheAction(int maker) : base(maker)
        {
            Maker = maker;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(Maker));
        }
    }
}