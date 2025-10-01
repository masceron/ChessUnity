using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Summon
{
    public class HorseLeech : PieceLogic, IPieceWithSkill
    {
        public HorseLeech(PieceConfig cfg) : base(cfg, KingMoves.Quiets, HorseLeechMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Piercing(-1, this)));
            Skills = list =>
            {
                if (SkillCooldown == 0) list.Add(new PhantomJellyActive(Pos));
            };
        }
        
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        
        public SkillsDelegate Skills { get; set; }
    }
}