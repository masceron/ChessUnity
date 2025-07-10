using Core.Effect;
using Core.General;
using Core.Piece;
using UnityEngine;

namespace Board.Action
{
    public class VelkarisMark: Action
    {
        public VelkarisMark(int p, ushort f, ushort t): base(p, false, false)
        {
            From = f;
            To = t;
        }

        public override void ApplyAction(GameState state)
        {
            Debug.Log("Velkaris marked " + To);
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            var caller = (Velkaris)state.MainBoard[From];
            state.MainBoard[To].Effects.Add(new VelkarisMarked(-1, 1, caller));
            caller.SkillCooldown = -1;
            caller.Marked = state.MainBoard[To];
            state.LastMove = this;
        }
    }
}