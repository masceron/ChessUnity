using Game.Common;
using MemoryPack;

namespace Game.Action
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class EndTurn : Action
    {
        public EndTurn()
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            BoardUtils.FlipSideToMove();
        }
    }
}