using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    public class MoveToDetach : Action, IDontEndTurn, IInternal
    {
        /// <summary>Reference đến quân ký sinh cần được tách ra và khôi phục vị trí.</summary>
        private readonly PieceLogic _parasite;

        public MoveToDetach(int maker, int target, PieceLogic parasite) : base(maker)
        {
            Target = target;
            _parasite = parasite;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.MoveToDetach(Maker, Target);

            var board = MatchManager.Ins.GameState.PieceBoard;
            board[Target] = _parasite;
            _parasite.Pos = Target;
        }
    }
}
