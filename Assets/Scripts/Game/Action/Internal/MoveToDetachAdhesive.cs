using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    /// <summary>
    ///     Spawn quân Adhesive trở lại bàn cờ khi host (Piece hoặc Formation) của nó bị phá hủy.<br/>
    ///     - Host là Piece (<paramref name="hostLogic"/> != null): dùng <see cref="PieceManager.MoveToDetach"/>.<br/>
    ///     - Host là Formation (<paramref name="hostLogic"/> == null): dùng <see cref="PieceManager.MoveToDetachFromFormation"/>
    ///       với key là vị trí formation.
    /// </summary>
    public class MoveToDetachAdhesive : Action, IInternal
    {
        /// <summary>Host PieceLogic — null nếu Adhesive bám vào Formation.</summary>
        private readonly PieceLogic _hostLogic;

        /// <summary>Reference đến quân Adhesive, dùng để cập nhật PieceBoard sau khi tách.</summary>
        private readonly PieceLogic _adhesive;

        /// <summary>Vị trí host/formation — dùng cho MoveToDetachFromFormation key.</summary>
        private readonly int _hostPos;

        /// <param name="hostPos">Vị trí host/formation vừa bị phá hủy.</param>
        /// <param name="target">Vị trí trống ngẫu nhiên xung quanh host để spawn Adhesive về.</param>
        /// <param name="adhesive">PieceLogic của quân Adhesive.</param>
        /// <param name="hostLogic">PieceLogic của host. Null nếu host là Formation.</param>
        public MoveToDetachAdhesive(int hostPos, int target, PieceLogic adhesive, PieceLogic hostLogic)
            : base(adhesive, target)
        {
            _hostPos = hostPos;
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
                // Host là Piece — dùng _parasiteMap[hostLogic]
                PieceManager.Ins.MoveToDetach(_hostLogic, GetTargetPos());
            }
            else
            {
                // Host là Formation — dùng _adhesiveFormationMap[hostPos]
                PieceManager.Ins.MoveToDetachFromFormation(_hostPos, GetTargetPos());
            }

            // Cập nhật PieceBoard logic
            var board = BoardUtils.PieceBoard();
            board[GetTargetPos()] = _adhesive;
            _adhesive.Pos = GetTargetPos();
        }
    }
}