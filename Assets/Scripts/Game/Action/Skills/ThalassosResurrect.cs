using System.Linq;
using Game.Action.Internal;
using Game.Data.Pieces;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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
            var color = ColorOfPiece(Maker);
            var collection = !color ? gameState.WhiteCaptured : gameState.BlackCaptured;
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(typeTo, color, Target)));

            collection.Remove(collection.First(e => e.Type == typeTo));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}