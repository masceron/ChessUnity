using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArcticBrittleStar : PieceLogic, IPieceWithSkill
    {
        public ArcticBrittleStar(PieceConfig cfg) : base(cfg, KingMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 15, this)));
            Skills = list =>
            {
                if (SkillCooldown > 0) return;
                list.Add(new ArcticBrittleStarActive(Pos, Pos));
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
    
}