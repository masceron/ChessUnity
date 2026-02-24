using Game.Action;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Petrified</b><br/>
    ///     Quân này không thể:<br/>
    ///     - Bị ảnh hưởng bởi Skill Target và AOE<br/>
    ///     - Di chuyển<br/>
    ///     - Capture<br/>
    ///     Nhưng <b>vẫn có thể bị ăn</b> (capture bởi quân địch) cho đến hết State Petrified.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Petrified : StateEffect
    {
        public override StateType StateType => StateType.Petrified;

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        public Petrified(int duration, PieceLogic piece)
            : base(duration, 0, piece, "effect_petrified")
        {
        }

    }
}
