using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class MaximaClamActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private MaximaClamActive()
        {
        }

        public MaximaClamActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            return pieceAI.Color != maker.Color ? -50 : 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetTargetAsPiece()));
            var maker = GetMakerAsPiece();
            maker.Quiets = GetTargetAsPiece().Quiets;
            SetCooldown(maker, ((IPieceWithSkill)maker).TimeToCooldown);
        }
    }
}