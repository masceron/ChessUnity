using MemoryPack;
using Game.Managers;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class DestroyConstruct: Action, ICaptures
    {
        [MemoryPackConstructor]
        private DestroyConstruct() { }

        public DestroyConstruct(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(Target);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Kill(Maker);
            MatchManager.Ins.GameState.Kill(Target);
        }
    }
}