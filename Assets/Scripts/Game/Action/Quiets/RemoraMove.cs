using Game.Managers;
using Game.Piece.PieceLogic.Commons;
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

        public RemoraMove(PieceLogic maker, int target) : base(maker, target)
        {}

        protected override void Animate()
        {
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            Move(GetMaker() as PieceLogic, GetTargetPos());
        }
    }
}