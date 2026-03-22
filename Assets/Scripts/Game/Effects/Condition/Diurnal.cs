using System.Collections.Generic;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Condition
{
    public class Diurnal : Effect, IOnMoveGenTrigger
    {
        public Diurnal(int duration, int strength, PieceLogic piece, string name) : base(duration, strength, piece, name)
        {
        }

        /// <summary>
        /// Trả về true nếu đang là ban ngày (IsDay). Các effect kế thừa Diurnal nên check biến này.
        /// </summary>
        public bool IsActive => Game.Managers.MatchManager.Ins != null && Game.Managers.MatchManager.Ins.GameState.IsDay;

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            if (IsActive) return;
            
            actions.RemoveAll(a => a is ISkills);
        }
    }
}
