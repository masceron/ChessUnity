using Game.Action.Internal;
using Game.Effects.Debuffs;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class IllusionCapture : Action, ICaptures
    {
        [MemoryPackConstructor]
        private IllusionCapture()
        {
        }

        public IllusionCapture(int maker, int target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, GetTarget()), GetMaker()));
        }
    }
}