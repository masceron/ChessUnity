using Game.Common;
using Game.Managers;
using MemoryPack;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class NormalMove : Action, IQuiets
    {
        [MemoryPackConstructor]
        private NormalMove()
        {
        }

        public NormalMove(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            BoardUtils.Move(Maker, Target);
        }
    }
}