using Game.Effects.Others;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMark : Action, IInternal
    {
        public VelkarisMark(PieceLogic f, PieceLogic t) : base(f, t)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new VelkarisMarked(GetTargetAsPiece()), GetMakerAsPiece()));

            SetCooldown(GetMakerAsPiece(), 0);
            ((Velkaris)GetMakerAsPiece()).Marked = GetTargetAsPiece();
        }
    }
}