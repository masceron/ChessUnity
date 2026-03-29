using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
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

        public NormalMove(int maker, int target) : base((PieceLogic)maker, (PieceLogic)target)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            BoardUtils.Move(GetMaker(), GetTargetPos());
        }
    }
}