using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenActive: Action, ISkills
    {
        public SirenActive(ushort caller, int f, int t) : base(caller, true)
        {
            From = (ushort)f;
            To = (ushort)t;
        }
        protected override void Animate()
        {
            PieceManager.Ins.Move(From, To);
        }

        protected override void ModifyGameState()
        {
            var gameState = MatchManager.Ins.GameState;
            gameState.Move(From, To);
            FlipPieceColor(To);
            SetCooldown(Caller, ((IPieceWithSkill)PieceOn(Caller)).TimeToCooldown);
        }
    }
}