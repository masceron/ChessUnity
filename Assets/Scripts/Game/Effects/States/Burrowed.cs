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
    public class Burrowed : StateEffect, IOnMoveGenTrigger
    {
        public override StateType StateType => StateType.Burrowed;

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        public Burrowed(int duration, PieceLogic piece)
            : base(duration, 0, piece, "effect_burrowed")
        {
        }

        /// <summary>
        /// 1. Không thể bị ăn và không thể thực hiện ăn nên capture xóa
        /// 2. Không thể di chuyển nên nếu là Iquiets và caller là nó thì xóa
        /// 3. Không bị ảnh hưởng bởi Skill Unit Target nhưng vẫn bị ảnh hưởng bởi Skill Localtion Target
        /// nên nếu là skill và không phải là Localtion Target thì xóa
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="actions"></param>
        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            foreach (var action in actions)
            {
                if (action is ICaptures) actions.Remove(action);
                if (action is IQuiets && caller == Piece) actions.Remove(action);
                if (action is ISkills && action.GetTargetPos() == Piece.Pos && action is not ILocaltionTarget) actions.Remove(action);
            }
        }
    }
}
