using static Game.Board.General.MatchManager;
using Color = Game.Board.General.Color;

namespace Game.Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EndTurn: Action
    {
        public EndTurn(): base(-1, false)
        {
            From = 0;
            To = 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            gameState.SideToMove = gameState.SideToMove == Color.White ? Color.Black : Color.White;
        }
    }
}