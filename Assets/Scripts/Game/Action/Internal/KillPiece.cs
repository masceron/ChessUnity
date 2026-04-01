using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    public class KillPiece : Action, IInternal
    {
        public KillPiece(PieceLogic maker) : base(maker)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(GetFrom());
            BoardUtils.KillPiece(GetMakerAsPiece());
        }
    }
}