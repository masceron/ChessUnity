using Game.Piece.PieceLogic;
using Game.Movesets;
using Game.Action.Skills;
using Game.Action;


namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Humilitas: PieceLogic, IPieceWithSkill
    {
        public Humilitas(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                list.Add(new HumilitasActive(Pos));
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
