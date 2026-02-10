using Game.Common;
using Game.Managers;
using MemoryPack;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class NormalSwap : Action, IQuiets
    {
        public NormalSwap(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Swap(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Swap(Maker, Target);
        }
    }
}
