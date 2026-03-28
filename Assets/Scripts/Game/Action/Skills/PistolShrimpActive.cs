using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class PistolShrimpActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private PistolShrimpActive()
        {
        }

        public PistolShrimpActive(int maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -80;
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetTargetPos()));
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}