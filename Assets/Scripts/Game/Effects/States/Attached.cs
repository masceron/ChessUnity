using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Attached</b><br/>
    ///     Quân này (hoặc Formation) đang chứa 1 quân có State <see cref="StateType.Adhesive"/>.<br/>
    ///     - Nhận hiệu ứng từ quân Adhesive gây ra (theo mô tả Skill).<br/>
    ///     - Khi bị ăn/phá hủy (<see cref="IDeadTrigger"/>): quân Adhesive được spawn ra
    ///       vị trí ngẫu nhiên còn trống xung quanh vị trí đó.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Attached : StateEffect, IDeadTrigger
    {
        /// <summary>Tham chiếu đến quân Adhesive đang bám.</summary>
        public PieceLogic AdhesivePiece;

        public override StateType StateType => StateType.Attached;

        public Attached(PieceLogic piece, PieceLogic adhesivePiece)
            : base(-1, 0, piece, "effect_attached")
        {
            AdhesivePiece = adhesivePiece;
        }

        /// <summary>
        ///     Khi quân Attached bị ăn/phá hủy: spawn quân Adhesive ra vị trí ngẫu nhiên
        ///     còn trống xung quanh vị trí vừa chết.
        /// </summary>
        public void OnCallDead(PieceLogic pieceToDie)
        {

        }
    }
}
