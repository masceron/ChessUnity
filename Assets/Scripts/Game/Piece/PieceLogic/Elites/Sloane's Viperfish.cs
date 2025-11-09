using Game.Action.Skills;
using Game.Movesets;
namespace Game.Piece.PieceLogic.Elites
{
    public class SloaneSViperfish : PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public SloaneSViperfish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown == 0) list.Add(new SloaneSViperfishActive(Pos));
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}