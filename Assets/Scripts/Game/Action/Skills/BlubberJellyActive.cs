using MemoryPack;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class BlubberJellyActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private BlubberJellyActive() { }

        public BlubberJellyActive(PieceLogic maker, int target) : base(maker, target)
        {
        }
        protected override void ModifyGameState()
        {
            var maker = GetMakerAsPiece();
            ActionManager.EnqueueAction(new NormalMove(maker, GetTargetPos()));
            
            var (rank, file) = RankFileOf(GetFrom());
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, maker.GetStat(SkillStat.Range, 2)))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null || pOn.Color == maker.Color) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Fear(maker.GetStat(SkillStat.Duration, 2), pOn), maker));
            }
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}