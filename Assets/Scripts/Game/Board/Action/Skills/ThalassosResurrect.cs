using System.Linq;
using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Board.Piece;
using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Skills
{
    public class ThalassosResurrect: Action, ISkills
    {
        private readonly PieceType typeTo;
        public ThalassosResurrect(int caller, int to, PieceType typeTo) : base(caller, true)
        {
            From = (ushort)caller;
            To = (ushort)to;
            this.typeTo = typeTo;
        }

        protected override void ModifyGameState()
        {
            var color = gameState.PieceBoard[Caller].Color;
            var collection = color == Color.White ? gameState.WhiteCaptured : gameState.BlackCaptured;
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(typeTo, color, To)));

            collection.Remove(collection.First(e => e.Type == typeTo));
        }
    }
}