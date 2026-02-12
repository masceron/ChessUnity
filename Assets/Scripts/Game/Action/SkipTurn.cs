using MemoryPack;
namespace Game.Action
{
    [MemoryPackable]
    public partial class SkipTurn: Action
    {
        public SkipTurn() : base(-1)
        {}

        protected override void ModifyGameState()
        {}
    }
}