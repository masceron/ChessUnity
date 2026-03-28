using Game.Action.Internal;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class RottingScytheAction : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private RottingScytheAction()
        {
        }

        public RottingScytheAction(int maker) : base(maker)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetFrom()));
        }
    }
}