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
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
            BoardUtils.KillPiece(GetTargetAsPiece());
        }
    }
}