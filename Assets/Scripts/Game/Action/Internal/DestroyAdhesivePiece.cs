using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    /// <summary>
    ///     Phá hủy quân Adhesive khi host bị ăn/phá hủy nhưng không còn ô trống xung quanh.<br/>
    ///     - Host là Piece (<paramref name="hostLogic"/> != null): dùng <see cref="PieceManager.DestroyParasite"/>.<br/>
    ///     - Host là Formation (<paramref name="hostLogic"/> == null): dùng <see cref="PieceManager.DestroyAdhesiveOnFormation"/>
    ///       với key là vị trí formation lưu trong <c>Maker</c>.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DestroyAdhesivePiece : Action, IInternal
    {
        private readonly PieceLogic _adhesive;
        /// <summary>Host PieceLogic — null nếu Adhesive bám vào Formation.</summary>
        private readonly PieceLogic _hostLogic;
        /// <summary>Vị trí formation (chỉ dùng khi _hostLogic == null).</summary>
        private readonly int _formationPos;

        /// <param name="adhesive">PieceLogic của quân Adhesive cần hủy.</param>
        /// <param name="hostLogic">PieceLogic của host. Null nếu host là Formation.</param>
        /// <param name="formationPos">Vị trí của Formation host (dùng khi hostLogic == null).</param>
        public DestroyAdhesivePiece(PieceLogic adhesive, PieceLogic hostLogic, int formationPos = -1)
            : base(null)
        {
            _adhesive     = adhesive;
            _hostLogic    = hostLogic;
            _formationPos = formationPos;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            // Dọn data: xóa observers của adhesive khỏi TriggerHooks
            _adhesive.Effects.ForEach(BoardUtils.GetTriggerHooks().RemoveObserver);

            if (_hostLogic != null)
            {
                // Host là Piece
                PieceManager.Ins.DestroyParasite(_hostLogic);
            }
            else
            {
                // Host là Formation
                PieceManager.Ins.DestroyAdhesiveOnFormation(_formationPos);
            }
        }
    }
}
