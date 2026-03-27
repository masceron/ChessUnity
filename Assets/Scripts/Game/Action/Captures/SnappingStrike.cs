using Game.Managers;
using MemoryPack;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SnappingStrike : Action, ICaptures
    {
        [MemoryPackConstructor]
        private SnappingStrike()
        {
        }

        public SnappingStrike(int maker, int target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
            MatchManager.Ins.GameState.Kill(GetTarget());
        }
    }
}