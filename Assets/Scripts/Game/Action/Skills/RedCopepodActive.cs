using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RedCopepodActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private RedCopepodActive()
        {
        }

        public RedCopepodActive(PieceLogic maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(GetMakerAsPiece(), GetTargetPos()));
        }
    }
}