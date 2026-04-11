using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FlyingGurnard : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Duration = 3;
        private const int Duration2 = 3;
        public FlyingGurnard(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, FlyingFishMoves.Captures)
        {
            SetStat(SkillStat.Duration, Duration);
            SetStat(SkillStat.Duration, Duration2, 2);
            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer) list.Add(new FlyingGurnardActive(this));
                //query for AI in here
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}