
using System.Collections.Generic;
using Game.Action.Captures;
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
    public class Ethereal : StateEffect, IOnMoveGenTrigger
    {
        public override StateType StateType => StateType.Ethereal;

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        public Ethereal(int duration, PieceLogic piece)
            : base(duration, 0, piece, "effect_ethereal")
        {
        }
    
        /// <summary>
        ///     Chặn mọi Capture action liên quan đến quân này (cả maker lẫn target).
        /// </summary>
            
        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            actions.RemoveAll(a => a is ICaptures && a.GetMaker() == caller || a.GetTarget() == caller);
        }
    }
}
