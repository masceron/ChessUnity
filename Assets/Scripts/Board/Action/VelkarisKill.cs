using Board.Interaction;
using Core;
using UnityEngine;

namespace Board.Action
{
    public class VelkarisKill: Action
    {
        public VelkarisKill(int f, int t)
        {
            From = f;
            To = t;
            Move = new Move
            {
                from = (byte)f,
                to = (byte)t,
                flag = MoveFlag.VelkarisKill
            };
        }
        public override void ApplyAction()
        {
            Object.Destroy(InteractionManager.pieceManager.GetPiece(To).gameObject);
            
            ActionManager.Execute(InteractionManager.gameState, new SwitchSide());
        }
    }
}