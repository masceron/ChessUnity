using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Securing</b><br/>
    ///     Mô tả chi tiết chưa được xác định — skeleton để triển khai sau.<br/>
    ///     Hiệu ứng cụ thể sẽ dựa trên mô tả Skill của quân cast state này.
    /// </summary>
    // TODO: Cập nhật logic khi có mô tả đầy đủ từ design doc
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Securing : StateEffect
    {
        public override StateType StateType => StateType.Securing;

        public Securing(PieceLogic piece) : base(-1, 0, piece, "effect_securing")
        {
        }
    }
}
