using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SwordFishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private SwordFishActive()
        {
        }

        public SwordFishActive(PieceLogic maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SnappingStrike(GetMakerAsPiece(), 1), GetMakerAsPiece()));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}