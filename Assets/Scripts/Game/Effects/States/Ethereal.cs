using Game.Action;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Ethereal</b><br/>
    ///     Quân này không thể:<br/>
    ///     - Bị Capture<br/>
    ///     - Capture quân khác<br/>
    ///     Nhưng vẫn có thể di chuyển và cast skill cho đến hết State Ethereal.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Ethereal : StateEffect, IBeforePieceActionTrigger
    {
        public override StateType StateType => StateType.Ethereal;

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        public Ethereal(int duration, PieceLogic piece)
            : base(duration, 0, piece, "state_ethereal")
        {
        }

        /// <summary>
        ///     Chặn mọi Capture action liên quan đến quân này (cả maker lẫn target).
        /// </summary>
        public void OnCallBeforePieceAction(Action.Action action)
        {

        }
    }
}
