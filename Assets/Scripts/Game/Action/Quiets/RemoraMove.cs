using MemoryPack;
using Game.Managers;
using static Game.Common.BoardUtils;

namespace Game.Action.Quiets
{
    [MemoryPackable]
    public partial class RemoraMove: Action, IQuiets
    {
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