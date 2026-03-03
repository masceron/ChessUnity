using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class BlubberJellyActive : Action, ISkills
    {
        public BlubberJellyActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(Maker, Target));
            var maker = PieceOn(Maker);
            
            var (rank, file) = RankFileOf(Maker);
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