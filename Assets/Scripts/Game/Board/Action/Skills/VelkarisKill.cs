using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisKill: Action, ISkills
    {
        public VelkarisKill(int p, ushort f, ushort t) : base(p)
        {
            Maker = f;
            Target = t;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(Target);
        }

        protected override void ModifyGameState()
        {
            var gameState = MatchManager.Ins.GameState;
            gameState.Destroy(Target);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}