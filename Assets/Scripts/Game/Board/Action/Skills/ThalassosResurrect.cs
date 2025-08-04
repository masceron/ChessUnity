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
        public ThalassosResurrect(int maker, int to, PieceType typeTo) : base(maker, true)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            this.typeTo = typeTo;
        }

        protected override void ModifyGameState()
        {
            var gameState = MatchManager.Ins.GameState;
            var color = BoardUtils.ColorOfPiece(Maker);
            var collection = !color ? gameState.WhiteCaptured : gameState.BlackCaptured;
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(typeTo, color, Target)));

            collection.Remove(collection.First(e => e.Type == typeTo));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}