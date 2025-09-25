using Game.Action.Skills;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Commons
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Archerfish: PieceLogic, IPieceWithSkill
    {
        public Archerfish(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown == 0) list.Add(new ArcherfishActive(Pos));
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }

}

