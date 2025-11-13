using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenActive: Action, ISkills
    {
        private readonly ushort moveTo;
        public SirenActive(ushort maker, int f, int t) : base(maker, true)
        {
            Maker = maker;
            Target = (ushort)f;
            moveTo = (ushort)t;
        }
        protected override void Animate()
        {
            PieceManager.Ins.Move(Target, moveTo);
        }

        protected override void ModifyGameState()
        {
            var gameState = MatchManager.Ins.GameState;
            gameState.Move(Target, moveTo);
            
            FlipPieceColor(moveTo);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}