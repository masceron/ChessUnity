using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisKill: Action, ISkills
    {
        public VelkarisKill(int p, ushort f, ushort t) : base(p, false)
        {
            From = f;
            To = t;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(To);
        }

        protected override void ModifyGameState()
        {
            var gameState = MatchManager.Ins.GameState;
            gameState.Destroy(To);
            SetCooldown(From, ((IPieceWithSkill)PieceOn(From)).TimeToCooldown);
        }
    }
}