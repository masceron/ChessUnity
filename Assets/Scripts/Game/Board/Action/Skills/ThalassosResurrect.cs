using System.Linq;
using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using Game.Common;
using static Game.Common.BoardUtils;

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
            var gameState = MatchManager.Ins.GameState;
            var color = BoardUtils.ColorOfPiece(Caller);
            var collection = !color ? gameState.WhiteCaptured : gameState.BlackCaptured;
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(typeTo, color, To)));

            collection.Remove(collection.First(e => e.Type == typeTo));
            SetCooldown(Caller, ((IPieceWithSkill)PieceOn(Caller)).TimeToCooldown);
        }
    }
}