using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Managers;
using Game.Triggers;

namespace Game.Effects.Condition
{
    /// <summary>
    /// Territorial + threshold + ...
    /// Áp dụng hiệu ứng khi có >= threshold quân địch ở phần sân mình.
    /// Subclass override OnTerritorialActivated() và OnTerritorialDeactivated() để áp dụng / gỡ bỏ hiệu ứng.
    /// Tự động gọi RefreshActive() mỗi cuối turn.
    /// </summary>
    public class Territorial : Effect, IEndTurnTrigger
    {
        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;
        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            RefreshActive();
        }
        /// <summary>Số lượng quân địch tối thiểu trên sân mình để kích hoạt.</summary>
        protected int Threshold;

        /// <summary>Trạng thái kích hoạt hiện tại.</summary>
        public bool IsActive { get; private set; }

        public Territorial(int threshold, PieceLogic piece, string effectName)
            : base(-1, 1, piece, effectName)
        {
            Threshold = threshold;
            IsActive = false;
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        /// <summary>
        /// Đếm số quân địch đang ở phần sân của quân this.Piece.
        /// </summary>
        private int CountEnemiesOnMySide()
        {
            var count = 0;
            var startingSize = MatchManager.Ins.StartingSize;
            var rankStart = (BoardUtils.MaxLength - startingSize.x) / 2;
            var fileStart = (BoardUtils.MaxLength - startingSize.y) / 2;
            var midRank = rankStart + startingSize.x / 2;

            if (!Piece.Color) // White: sân mình = ranks >= midRank
            {
                for (var r = midRank; r < rankStart + startingSize.x; r++)
                for (var f = fileStart; f < fileStart + startingSize.y; f++)
                {
                    var p = BoardUtils.PieceOn(BoardUtils.IndexOf(r, f));
                    if (p != null && p.Color != Piece.Color) count++;
                }
            }
            else // Black: sân mình = ranks < midRank
            {
                for (var r = rankStart; r < midRank; r++)
                for (var f = fileStart; f < fileStart + startingSize.y; f++)
                {
                    var p = BoardUtils.PieceOn(BoardUtils.IndexOf(r, f));
                    if (p != null && p.Color != Piece.Color) count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Gọi hàm này (cuối turn) để cập nhật trạng thái kích hoạt.
        /// </summary>
        protected void RefreshActive()
        {
            var newActive = CountEnemiesOnMySide() >= Threshold;
            if (newActive == IsActive) return;

            IsActive = newActive;
            if (IsActive)
                OnTerritorialActivated();
            else
                OnTerritorialDeactivated();
        }

        /// <summary>Gọi khi vừa kích hoạt (đủ quân địch trên sân mình). Override ở subclass.</summary>
        protected virtual void OnTerritorialActivated() { }

        /// <summary>Gọi khi hết kích hoạt (quân địch rút đi). Override ở subclass.</summary>
        protected virtual void OnTerritorialDeactivated() { }
    }
}
