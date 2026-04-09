using MemoryPack;

namespace Game.Action
{
    [MemoryPackable]
    public partial class SkipTurn : Action
    {
        public SkipTurn() : base(null)
        {
        }

        protected override void ModifyGameState()
        {
        }
    }
}