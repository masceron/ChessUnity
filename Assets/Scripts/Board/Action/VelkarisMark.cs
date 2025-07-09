using Core;
using UnityEngine;

namespace Board.Action
{
    public class VelkarisMark: Action
    {
        public VelkarisMark(int p, int f, int t): base(p)
        {
            From = (ushort)f;
            To = (ushort)t;
        }

        public override void ApplyAction(GameState state)
        {
            Debug.Log("Velkaris marked " + To);
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.LastMovedPiece = state.MainBoard[From];
            state.MainBoard[To].Effects.Add(new Effect(EffectType.VelkarisMarked, -1, 1));
            state.MainBoard[From].SkillCooldown = -1;
            state.LastMove = this;
        }

        public override bool DoesMoveChangePos()
        {
            return false;
        }
    }
}