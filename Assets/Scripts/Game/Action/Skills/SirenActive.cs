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

        public SirenActive(PieceLogic maker, PieceLogic target, int moveTo) : base(maker, target)
        {
            _moveTo = moveTo;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -50 : 0;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(GetTargetPos(), _moveTo);
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new SirenForceMove(GetTargetAsPiece(), _moveTo));
            
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
    
    internal class SirenForceMove: Action
    {
        public SirenForceMove(PieceLogic maker, int target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            Move(GetMakerAsPiece(), GetTargetPos());
            FlipPieceColor(GetMakerAsPiece());
        }
    }
}