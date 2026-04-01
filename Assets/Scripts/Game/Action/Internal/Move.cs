using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    public class Move: Action, IInternal
    {
        public Move(PieceLogic maker, int target) : base(maker, target)
        {
            
        }
        protected override void ModifyGameState()
        {
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
            BoardUtils.Move(GetMakerAsPiece(), GetTargetPos());
        }
    }
}