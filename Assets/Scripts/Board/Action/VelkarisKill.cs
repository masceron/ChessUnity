using Board.Interaction;
using Core;
using UnityEngine;

namespace Board.Action
{
    public class VelkarisKill: Action
    {
        public VelkarisKill(int f, int t)
        {
            From = (ushort)f;
            To = (ushort)t;
        }

        public override void ApplyAction(GameState state)
        {
            Object.Destroy(InteractionManager.PieceManager.GetPiece(To).gameObject);
            ActionManager.Execute(InteractionManager.GameState, new SwitchSide());
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.RemoveTrigger(state.MainBoard[To]);
            state.MainBoard[To] = null;
            state.MainBoard[From].SkillCooldown = -1;
            state.LastMove = this;
        }

        public override bool DoesMoveChangePos()
        {
            return false;
        }
    }
}