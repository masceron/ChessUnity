using Board.Interaction;
using Core.General;

namespace Board.Action
{
    public class EndTurn: Action
    {
        public EndTurn(): base(-1, false, false)
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
    }
}