using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using System.Collections.Generic;
using static Game.Common.BoardUtils;

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
    public class Petrified : StateEffect, IOnMoveGenTrigger, IBeforePieceActionTrigger
    {
        public override StateType StateType => StateType.Petrified;

        public BeforeActionPriority Priority => BeforeActionPriority.Reaction;

        public Petrified(int duration, PieceLogic piece)
            : base(duration, 0, piece, "effect_petrified")
        {
        }

        /// <summary>
        /// 1. Xóa hành động di chuyển và ăn quân của chính nó
        /// 2. Xóa các Skill tác dụng lên nó (không chặn ILocaltionTarget và sẽ chặn IAOE ở Before Action)
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="actions"></param>
        void IOnMoveGenTrigger.OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece)
            {
                actions.RemoveAll(a => 
                        a is IQuiets 
                        || a is ICaptures);
            }

            foreach (var action in actions)
            {
                if (action is ISkills && PieceOn(action.Target) == Piece && action is not ILocaltionTarget)
                {
                    actions.Remove(action);
                }
            }
        }

        /// <summary>
        /// Chặn tác dụng của ILocaltionTarget sinh ra
        /// </summary>
        /// <param name="action"></param>
        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is IAOE && action.Target == Piece.Pos) 
            {
                action.Result = Action.ResultFlag.Failed;
            }
        }
    }
}
