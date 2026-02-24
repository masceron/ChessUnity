using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Adhesive</b><br/>
    ///     Khi quân này Capture vào quân địch hoặc Formation có State <see cref="StateType.None"/>:<br/>
    ///     - Quân này biến mất tạm thời (di chuyển sang bên cạnh bàn cờ).<br/>
    ///     - State của target (quân hoặc formation) chuyển sang <see cref="StateType.Attached"/>.<br/>
    ///     - Khi target bị ăn/phá hủy: quân Adhesive xuất hiện lại ở vị trí ngẫu nhiên trống xung quanh.<br/>
    ///     Quân Adhesive chỉ thực hiện hành động này lên các quân có State là <see cref="StateType.None"/>.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Adhesive : StateEffect, IBeforePieceActionTrigger
    {
        public override StateType StateType => StateType.Adhesive;

        public BeforeActionPriority Priority => BeforeActionPriority.Declaration;

        public Adhesive(PieceLogic piece) : base(-1, 0, piece, "effect_adhesive")
        {
        }

        /// <summary>
        ///     Nếu action là Capture vào quân/formation có State None:
        ///     chặn capture thông thường, thay bằng AdhesiveCapture.
        /// </summary>
        public void OnCallBeforePieceAction(Action.Action action)
        {

        }
    }
}
