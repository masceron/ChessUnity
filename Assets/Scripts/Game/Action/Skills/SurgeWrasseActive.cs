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
            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(GetFrom()), FileOf(GetFrom()), 1))
            {
                var pieceOn = PieceOn(IndexOf(rank, file));
                if (pieceOn != null && pieceOn.Color == GetMakerAsPiece().Color)
                    ActionManager.EnqueueAction(new ApplyEffect(new LongReach(pieceOn, 2, 2)));
            }

            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}