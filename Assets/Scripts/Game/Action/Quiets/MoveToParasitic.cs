using MemoryPack;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Quiets
{
    [MemoryPackable]
    public partial class MoveToParasitic : Action
    {
        [MemoryPackConstructor]
        private MoveToParasitic() { }

        public MoveToParasitic(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            var hostLogic = GetTargetAsPiece();
            PieceManager.Ins.MoveToParasitic(GetFrom(), GetTargetPos(), hostLogic);

            var parasite = GetMakerAsPiece();
            if (parasite == null) return;

            BoardUtils.PieceBoard()[GetFrom()] = null;
            parasite.Pos = -9999;
        }
    }
}