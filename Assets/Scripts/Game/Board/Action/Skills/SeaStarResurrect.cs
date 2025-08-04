using System.Linq;
using Game.Board.Action.Internal;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class SeaStarResurrect: Action, ISkills
    {
        public SeaStarResurrect(int maker, int to) : base(maker, true)
        {
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            var caller = PieceOn(Maker);
            var collection = !caller.Color ? WhiteCaptured() : BlackCaptured();
            
            ActionManager.ExecuteImmediately(new SpawnPiece(new PieceConfig(PieceType.SeaStar, caller.Color, Target)));
            collection.Remove(collection.First(p => p.Type == PieceType.SeaStar));
            SetCooldown(Target, -1);
            SetCooldown(Maker, ((IPieceWithSkill) PieceOn(Maker)).TimeToCooldown);
        }
    }
}