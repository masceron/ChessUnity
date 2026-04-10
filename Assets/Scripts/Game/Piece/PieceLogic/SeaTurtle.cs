using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtle : Commons.PieceLogic, IPieceWithSkill
    {
        public SeaTurtle(PieceConfig cfg) : base(cfg, KingMoves.Quiets, FrontDefenderMoves.Captures)
        {
            SetStat(SkillStat.Duration, 2);
            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer) list.Add(new SeaTurtleActive(this));
                //query for AI in here
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}