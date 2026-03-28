using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Skills;
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
    public class Burrowed : StateEffect, IBeforePieceActionTrigger, IOnMoveGenTrigger
    {
        public override StateType StateType => StateType.Burrowed;

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        public Burrowed(int duration, PieceLogic piece)
            : base(duration, 0, piece, "effect_burrowed")
        {
        }

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller == Piece)
            {
                for (var i = actions.Count - 1; i >= 0; i--)
                    if (actions[i] is IQuiets || actions[i] is ICaptures)
                        actions.RemoveAt(i);
            } else {
                for (var i = actions.Count - 1; i >= 0; i--)
                {
                    var act = actions[i];
                    if (act is ISkills && act.Target == Piece.Pos)
                        actions.RemoveAt(i);
                }
            }
        }

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action == null || action.Result != ResultFlag.Success) return;

            if ((action is IQuiets || action is ICaptures) && action.GetMaker() == Piece)
            {
                action.Result = ResultFlag.Blocked;
                return;
            }

            if (action is ICaptures && action.Target == Piece.Pos)
            {
                action.Result = ResultFlag.Blocked;
                return;
            }
        }
    }
}
