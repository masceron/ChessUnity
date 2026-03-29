using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SirenActive : Action, ISkills
    {
        [MemoryPackInclude] private int _moveTo;

        [MemoryPackConstructor]
        private SirenActive()
        {
        }

        public SirenActive(int maker, int target, int moveTo) : base(maker, target)
        {
            _moveTo = moveTo;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker() as PieceLogic;
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -50 : 0;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(GetTargetPos(), _moveTo);
        }

        protected override void ModifyGameState()
        {
            Move(GetTarget(), _moveTo);

            FlipPieceColor(PieceOn(_moveTo));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker() as PieceLogic).TimeToCooldown);
        }
    }
}