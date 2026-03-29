using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    /// <summary>
    ///     Spawn quân Adhesive trở lại bàn cờ khi host (Piece hoặc Formation) của nó bị phá hủy.<br/>
    ///     - Host là Piece (<paramref name="hostLogic"/> != null): dùng <see cref="PieceManager.MoveToDetach"/>.<br/>
    ///     - Host là Formation (<paramref name="hostLogic"/> == null): dùng <see cref="PieceManager.MoveToDetachFromFormation"/>
    ///       với key là <paramref name="maker"/> (vị trí formation).
    /// </summary>
    public class MoveToDetachAdhesive : Action, IInternal
    {
        /// <summary>Host PieceLogic — null nếu Adhesive bám vào Formation.</summary>
        private readonly PieceLogic _hostLogic;

        /// <summary>Reference đến quân Adhesive, dùng để cập nhật PieceBoard sau khi tách.</summary>
        private readonly PieceLogic _adhesive;

        /// <param name="maker">Vị trí tách ra (vị trí host/formation vừa bị phá hủy).</param>
        /// <param name="target">Vị trí trống ngẫu nhiên xung quanh host để spawn Adhesive về.</param>
        /// <param name="adhesive">PieceLogic của quân Adhesive.</param>
        /// <param name="hostLogic">PieceLogic của host. Null nếu host là Formation.</param>
        public MoveToDetachAdhesive(int maker, int target, PieceLogic adhesive, PieceLogic hostLogic)
            : base((PieceLogic)maker, (PieceLogic)target)
        {
            _adhesive = adhesive;
            _hostLogic = hostLogic;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            if (_hostLogic != null)
            {
                // Host là Piece — dùng PieceManager thông thường
                PieceManager.Ins.MoveToDetach(_hostLogic, GetTargetPos());
            }
            else
            {
                // Host là Formation — dùng map theo vị trí (Maker = vị trí formation)
                PieceManager.Ins.MoveToDetachFromFormation(GetMakerPos(), GetTargetPos());
            }

            // Cập nhật PieceBoard logic
            var board = MatchManager.Ins.GameState.PieceBoard;
            board[GetTargetPos()] = _adhesive;
            _adhesive.Pos = GetTargetPos();
        }
    }
}