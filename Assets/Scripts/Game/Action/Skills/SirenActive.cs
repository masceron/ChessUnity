using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenActive: Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -50 : 0;
        }
        private readonly int _moveTo;
        public SirenActive(int maker, int f, int t) : base(maker)
        {
            Maker = maker;
            Target = f;
            _moveTo = t;
        }
        protected override void Animate()
        {
            PieceManager.Ins.Move(Target, _moveTo);
        }

        protected override void ModifyGameState()
        {
            Move(Target, _moveTo);
            
            FlipPieceColor(_moveTo);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}