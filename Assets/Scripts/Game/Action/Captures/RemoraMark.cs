using Game.Action.Internal;
using Game.Effects.Others;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RemoraMark : Action, ICaptures
    {
        [MemoryPackConstructor]
        private RemoraMark()
        {
        }

        public RemoraMark(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new RemoraMarked(PieceOn(Maker), PieceOn(Target)),
                PieceOn(Maker)));
        }
    }
}