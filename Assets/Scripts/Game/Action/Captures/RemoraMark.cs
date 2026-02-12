using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Others;
using static Game.Common.BoardUtils;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RemoraMark: Action, ICaptures
    {
        public RemoraMark(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new RemoraMarked(PieceOn(Maker), PieceOn(Target)), PieceOn(Maker)));
        }
    }
}