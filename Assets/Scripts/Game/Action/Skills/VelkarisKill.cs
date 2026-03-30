using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class VelkarisKill : Action, ISkills
    {
        [MemoryPackConstructor]
        private VelkarisKill()
        {
        }

        public VelkarisKill(int f, int t) : base(f, t)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetTargetPos()));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}