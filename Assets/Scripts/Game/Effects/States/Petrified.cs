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
    public class Petrified : StateEffect, IOnMoveGenTrigger
    {
        public override StateType StateType => StateType.Petrified;

        public Petrified(int duration, PieceLogic piece)
            : base(duration, 0, piece, "effect_petrified")
        {
        }

        void IOnMoveGenTrigger.OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            foreach (var action in actions)
            {
                if (action is ISkills && action.GetTarget() == Piece)
                {
                    actions.Remove(action);
                }

                if (action.GetMaker() == Piece) {
                    if (action is IQuiets || action is ICaptures)
                    {
                        actions.Remove(action);
                    }
                }
            }
        }
    }
}
