using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Managers;
using Game.Triggers;

namespace Game.Effects.Condition
{
    /// <summary>
    /// Invasive + ...
    /// Áp dụng hiệu ứng khi quân này đang ở phần sân địch.
    /// Subclass override OnInvasiveActivated() và OnInvasiveDeactivated() để áp dụng / gỡ bỏ hiệu ứng.
    /// Tự động gọi RefreshActive() mỗi cuối turn.
    /// </summary>
    public class Invasive : Effect, IEndTurnTrigger
    {
        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;
        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            RefreshActive();
        }

        /// <summary>Trạng thái kích hoạt hiện tại.</summary>
        public bool IsActive { get; private set; }

        public Invasive(PieceLogic piece, string effectName)
            : base(-1, 1, piece, effectName)
        {
            IsActive = false;
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        /// <summary>
        /// Kiểm tra xem quân cờ có đang ở phần sân địch hay không.
        /// White ở bên >= midRank, nên sân địch của White = ranks < midRank
        /// Black ở bên < midRank, nên sân địch của Black = ranks >= midRank
        /// </summary>
        private bool IsOnEnemySide()
        {
            var startingSize = MatchManager.Ins.StartingSize;
            var rankStart = (BoardUtils.MaxLength - startingSize.x) / 2;
            var midRank = rankStart + startingSize.x / 2;
            var myRank = BoardUtils.RankOf(Piece.Pos);

            if (!Piece.Color) // White: sân địch = ranks < midRank
                return myRank < midRank;
            else // Black: sân địch = ranks >= midRank
                return myRank >= midRank;
        }

        /// <summary>
        /// Gọi hàm này (sau khi di chuyển) để cập nhật trạng thái kích hoạt.
        /// </summary>
        protected void RefreshActive()
        {
            var newActive = IsOnEnemySide();
            if (newActive == IsActive) return;

            IsActive = newActive;
            if (IsActive)
                OnInvasiveActivated();
            else
                OnInvasiveDeactivated();
        }

        /// <summary>Gọi khi quân cờ vừa bước vào sân địch. Override ở subclass.</summary>
        protected virtual void OnInvasiveActivated() { }

        /// <summary>Gọi khi quân cờ rời sân địch (quay về sân mình). Override ở subclass.</summary>
        protected virtual void OnInvasiveDeactivated() { }
    }
}
