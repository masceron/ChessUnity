using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtle : PieceLogic, IPieceWithSkill
    {
        public SeaTurtle(PieceConfig cfg) : base(cfg, SeaTurtleMoves.Quiets, SeaTurtleMoves.Captures)
        { 
            Skills = list =>
            {
                if (SkillCooldown == 0) {
                    list.Add(new SeaTurtleActive(Pos));
                }
            };
            
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}