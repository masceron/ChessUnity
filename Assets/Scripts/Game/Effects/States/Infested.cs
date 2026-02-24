using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Infested</b><br/>
    ///     Quân này đang chứa 1 quân có State <see cref="StateType.Parasite"/>.<br/>
    ///     - Nhận hiệu ứng do quân Parasite gây ra (được xử lý bởi <see cref="Parasite"/>).<br/>
    ///     - Khi bị ăn hoặc phá hủy (<see cref="IDeadTrigger"/>): quân Parasite sẽ được
    ///       spawn ra vị trí ngẫu nhiên xung quanh chỗ vừa chết.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Infested : StateEffect, IDeadTrigger
    {
        /// <summary>Tham chiếu đến quân Parasite đang ký sinh.</summary>
        public PieceLogic ParasitePiece;

        public override StateType StateType => StateType.Infested;

        public Infested(PieceLogic piece, PieceLogic parasitePiece)
            : base(-1, 0, piece, "effect_infested")
        {
            ParasitePiece = parasitePiece;
        }

        /// <summary>
        ///     Khi quân Infested bị ăn/phá hủy: spawn quân Parasite ra vị trí ngẫu nhiên
        ///     xung quanh vị trí vừa chết.
        /// </summary>
        public void OnCallDead(PieceLogic pieceToDie)
        {
            
        }
    }
}
