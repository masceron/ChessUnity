using MemoryPack;
using Game.Managers;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SnappingStrike: Action, ICaptures
    {
        public SnappingStrike(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(Target);
            MatchManager.Ins.GameState.Kill(Target);
        }
    }
}