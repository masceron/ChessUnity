using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    public class KillPiece : Action, IInternal
    {
        public KillPiece(Entity maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            BoardUtils.KillPiece(GetTargetAsPiece());
        }
    }
}