using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    public class SwapPiece: Action, IInternal
    {
        public SwapPiece(PieceLogic first, PieceLogic second) : base(first, second)
        {
            
        }


        protected override void ModifyGameState()
        {
            BoardUtils.Swap(GetMakerAsPiece(), GetTargetAsPiece());
        }
    }
}