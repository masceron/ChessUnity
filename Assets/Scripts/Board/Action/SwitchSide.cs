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
        }

        public override void ApplyAction(GameState state)
        {
            InteractionManager.UnmarkPiece(InteractionManager.SelectingPiece);
            InteractionManager.SelectingPiece = -1;
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.SideToMove = state.SideToMove == Color.White ? Color.Black : Color.White;
            state.EndTurn();
        }

        public override bool DoesMoveChangePos()
        {
            return false;
        }
    }
}