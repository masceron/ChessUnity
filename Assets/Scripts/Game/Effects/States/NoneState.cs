using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.States
{
    /// <summary>
    ///     State mặc định — không có hiệu ứng gì.
    ///     Có thể dùng để reset State của quân cờ về None một cách tường minh.
    ///     Thông thường dùng <see cref="PieceLogic.ClearState"/> thay vì áp NoneState.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NoneState : StateEffect
    {
        public override StateType StateType => StateType.None;

        public NoneState(PieceLogic piece) : base(-1, 0, piece, "effect_none_state")
        {
        }
    }
}
