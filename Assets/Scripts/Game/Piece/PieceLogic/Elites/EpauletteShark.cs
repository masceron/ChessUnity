using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Others;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Elites
{
    public class EpauletteShark : PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;
        public EpauletteShark(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new EpauletteSharkPurify(this)));
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