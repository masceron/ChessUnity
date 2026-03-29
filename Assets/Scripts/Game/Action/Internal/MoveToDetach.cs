using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    public class MoveToDetach : Action, IInternal
    {
        /// <summary>Logic của quân bị ký sinh (host), dùng để tra cứu parasite Piece trong PieceManager.</summary>
        private readonly PieceLogic _hostLogic;

        /// <summary>Reference đến quân ký sinh, dùng để cập nhật PieceBoard sau khi tách.</summary>
        private readonly PieceLogic _parasite;

        public MoveToDetach(int maker, int target, PieceLogic parasite, PieceLogic hostLogic) : base((PieceLogic)maker, (PieceLogic)target)
        {
            _parasite = parasite;
            _hostLogic = hostLogic;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            // Tra cứu parasite Piece qua hostLogic rồi animate về Target
            PieceManager.Ins.MoveToDetach(_hostLogic, GetTargetPos());

            // Cập nhật PieceBoard logic
            var board = MatchManager.Ins.GameState.PieceBoard;
            board[GetTargetPos()] = _parasite;
            _parasite.Pos = GetTargetPos();
        }
    }
}
