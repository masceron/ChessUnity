using Game.Action;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Burrowed</b><br/>
    ///     Quân này không thể:<br/>
    ///     - Bị target bởi skill chọn quân (Target skill)<br/>
    ///     - Bị capture<br/>
    ///     - Di chuyển<br/>
    ///     - Ăn quân<br/>
    ///     Nhưng vẫn bị ảnh hưởng bởi skill AOE (có radius) cho đến hết State Burrowed.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Burrowed : StateEffect
    {
        public override StateType StateType => StateType.Burrowed;

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        public Burrowed(int duration, PieceLogic piece)
            : base(duration, 0, piece, "effect_burrowed")
        {
        }


    }
}
