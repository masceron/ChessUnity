using Game.Managers;
using MemoryPack;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class DestroyConstruct : Action, ICaptures
    {
        [MemoryPackConstructor]
        private DestroyConstruct()
        {
        }

        public DestroyConstruct(int maker, int target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Kill(GetMaker());
            MatchManager.Ins.GameState.Kill(GetTarget());
        }
    }
}