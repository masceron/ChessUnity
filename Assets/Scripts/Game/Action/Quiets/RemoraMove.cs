using Game.Managers;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Quiets
{
    [MemoryPackable]
    public partial class RemoraMove : Action, IQuiets
    {
        [MemoryPackConstructor]
        private RemoraMove()
        {
        }

        public RemoraMove(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            Move(Maker, Target);
            Maker = Target;
        }
    }
}