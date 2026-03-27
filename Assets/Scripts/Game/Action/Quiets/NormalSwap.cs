using Game.Managers;
using MemoryPack;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class NormalSwap : Action, IQuiets
    {
        [MemoryPackConstructor]
        private NormalSwap()
        {
        }

        public NormalSwap(int maker, int target) : base(maker, target, TargetingType.LocationTargeting)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Swap(GetMakerPos(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Swap(GetMaker(), GetTarget());
        }
    }
}