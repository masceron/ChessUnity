using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenActive: Action, ISkills
    {
        private readonly ushort moveFrom;
        public SirenActive(ushort from, int f, int t) : base(from, true)
        {
            From = from;
            moveFrom = (ushort)f;
            To = (ushort)t;
        }
        protected override void Animate()
        {
            PieceManager.Ins.Move(From, To);
        }

        protected override void ModifyGameState()
        {
            var gameState = MatchManager.Ins.GameState;
            gameState.Move(moveFrom, To);
            FlipPieceColor(To);
            SetCooldown(From, ((IPieceWithSkill)PieceOn(From)).TimeToCooldown);
        }
    }
}