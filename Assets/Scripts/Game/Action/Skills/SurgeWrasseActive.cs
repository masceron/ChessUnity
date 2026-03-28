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

        public SurgeWrasseActive(int maker) : base(maker)
        {
            Target = maker;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), 1))
            {
                var pieceOn = PieceOn(IndexOf(rank, file));
                if (pieceOn != null && pieceOn.Color == GetMaker().Color)
                    ActionManager.EnqueueAction(new ApplyEffect(new LongReach(pieceOn, 2, 2)));
            }

            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}