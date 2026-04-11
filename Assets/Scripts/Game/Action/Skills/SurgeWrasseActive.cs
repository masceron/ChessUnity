using Game.Action.Internal;
using Game.Common;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

// <-- thêm để dùng LINQ

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class SurgeWrasseActive : Action, ISkills
    {
        private const int Strength = 2;
        private const int Duration = 2;
        [MemoryPackConstructor]
        private SurgeWrasseActive()
        {
        }

        public SurgeWrasseActive(PieceLogic maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var listAllyPieces = SkillRangeHelper.GetActiveAllyPieceInRadius(GetFrom(), 1);
            foreach (var piece in listAllyPieces)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new LongReach(PieceOn(piece), Strength, Duration)));
            }

            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}