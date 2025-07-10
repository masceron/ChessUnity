using Board.Interaction;
using Core;
using Core.General;
using Core.Piece;
using UnityEngine;

namespace Board.Action
{
    public class VelkarisKill: Action
    {
        public VelkarisKill(int p, ushort f, ushort t) : base(p, false, false)
        {
            From = f;
            To = t;
        }

        public override void ApplyAction(GameState state)
        {
            Object.Destroy(InteractionManager.PieceManager.GetPiece(To).gameObject);
            ActionManager.Execute(new EndTurn());
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.RemoveTrigger(state.MainBoard[To]);
            state.MainBoard[To] = null;
            ((Velkaris)state.MainBoard[From]).SkillCooldown = -1;
            state.LastMove = this;
        }
    }
}