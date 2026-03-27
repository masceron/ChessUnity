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

        public RemoraMove(int maker, int target) : base(maker, target, TargetingType.LocationTargeting)
        {}

        protected override void Animate()
        {
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            Move(GetMaker(), GetTargetPos());
        }
    }
}