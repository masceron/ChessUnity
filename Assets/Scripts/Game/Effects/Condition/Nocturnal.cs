using System.Collections.Generic;
using Game.Action.Skills;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Condition
{
    public class Nocturnal : Effect, IOnMoveGenTrigger
    {
        public Nocturnal(int duration, int strength, PieceLogic piece, string name) : base(duration, strength, piece, name)
        {
        }

        /// <summary>
        /// Trả về true nếu đang là ban đêm (!IsDay). Các effect kế thừa Nocturnal nên check biến này.
        /// </summary>
        public static bool IsActive => Managers.MatchManager.Ins != null && !BoardUtils.IsDay();

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            if (IsActive) return;
            
            actions.RemoveAll(a => a is ISkills);
        }
    }
}
