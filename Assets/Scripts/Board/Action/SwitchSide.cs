using System;
using Board.Interaction;
using Core;

namespace Board.Action
{
    public class SwitchSide: Action
    {
        public SwitchSide()
        {
            From = 0;
            To = 0;
            
            Move = new Move
            {
                from = 0,
                to = 0,
                flag = MoveFlag.SwitchSide
            };
        }
        public override void ApplyAction()
        {
            InteractionManager.UnmarkPiece(InteractionManager.selectingPiece);
            InteractionManager.selectingPiece = -1;
            return;
        }
    }
}