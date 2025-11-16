using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    public class SloaneSViperfish : Commons.PieceLogic, IPieceWithSkill
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