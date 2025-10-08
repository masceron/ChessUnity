using Game.Action.Skills;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Elites
{
    public class EpauletteShark : PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;
        public EpauletteShark(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown == 0) list.Add(new EpauletteSharkActive(Pos));
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